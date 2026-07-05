using Godot;
using System;

namespace Rusty.ActionGraph;

/// <summary>
/// A color field.
/// </summary>
public partial class ColorField : HBoxContainer, IWidget, IValued<Color>
{
    /* Public properties. */
    public string Title
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    public int TitleWidth
    {
        get => (int)Label.CustomMinimumSize.X;
        set => Label.CustomMinimumSize = new(value, Label.CustomMinimumSize.Y);
    }
    public string Description
    {
        get => TooltipText;
        set
        {
            TooltipText = value;
            Label.TooltipText = value;
            ColorPickerButton.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo { get; set; }

    public Color Value => ColorPickerButton.Color;

    /* Private methods. */
    private Label Label { get; set; }
    private ColorPickerButton ColorPickerButton { get; set; }
    private Color OldColor { get; set; }
    private bool Cancelled { get; set; }

    /* Public events. */
    public event Action<IWidget> Changed;

    /* Constructors. */
    public ColorField()
    {
        Label = new();
        AddChild(Label);
        TitleWidth = 160;

        ColorPickerButton = new();
        ColorPickerButton.Color = Colors.White;
        ColorPickerButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        ColorPickerButton.Pressed += OnColorPickerOpened;
        ColorPickerButton.PopupClosed += OnColorPickerClosed;
        AddChild(ColorPickerButton);
        DisableHotkeys(ColorPickerButton.GetPopup());

        WidgetRegistry.Add(this);
    }

    public ColorField(Color color) : this()
    {
        ColorPickerButton.Color = color;
    }

    ~ColorField()
    {
        WidgetRegistry.Remove(this);
    }

    /* Public methods. */
    public IWidget Copy()
    {
        ColorField field = new();
        field.SizeFlagsHorizontal = SizeFlagsHorizontal;
        field.SizeFlagsVertical = SizeFlagsVertical;
        field.Title = Title;
        field.TitleWidth = TitleWidth;
        field.Description = Description;
        field.ColorPickerButton.Color = Value;
        field.UndoRedo = UndoRedo;
        field.OldColor = Value;
        return field;
    }

    public void ExpandFill(bool vertical = false)
    {
        SizeFlagsHorizontal = SizeFlags.ExpandFill;
        if (vertical)
            SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    public void SetValue(Color value)
    {
        SetValue(OldColor, value);
    }

    public void CancelFocus()
    {
        if (ColorPickerButton.GetPopup().Visible)
        {
            Cancelled = true;
            ColorPickerButton.ReleaseFocus();
            ColorPickerButton.GetPopup().Hide();
            ColorPickerButton.Color = OldColor;
            Cancelled = false;
        }
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventKey)
            AcceptEvent();
    }

    /* Private methods. */
    private void SetValue(Color from, Color to)
    {
        if (from == to)
            return;

        UndoRedo?.CreateAction($"Changed color {Title}: #{from.ToHtml()} \u25B6 #{to.ToHtml()}");

        UndoRedo?.AddUndoMethod(new Callable(this, nameof(CancelAll)));
        UndoRedo?.AddUndoProperty(ColorPickerButton, "color", from);
        UndoRedo?.AddUndoMethod(new Callable(this, nameof(InvokeChangedEvent)));

        UndoRedo?.AddDoMethod(new Callable(this, nameof(CancelAll)));
        UndoRedo?.AddDoProperty(ColorPickerButton, "color", to);
        UndoRedo?.AddDoMethod(new Callable(this, nameof(InvokeChangedEvent)));

        UndoRedo?.CommitAction(false);

        ColorPickerButton.Color = to;
        InvokeChangedEvent();
    }

    private void CancelAll()
    {
        WidgetRegistry.ReleaseFocus();
    }

    private void OnColorPickerOpened()
    {
        OldColor = ColorPickerButton.Color;
    }

    private void OnColorPickerClosed()
    {
        if (!Cancelled)
            SetValue(OldColor, ColorPickerButton.Color);
    }

    private void InvokeChangedEvent()
    {
        Changed?.Invoke(this);
    }

    private void DisableHotkeys(Node control)
    {
        if (control is TextEdit te)
            te.ShortcutKeysEnabled = false;
        if (control is LineEdit le)
            le.ShortcutKeysEnabled = false;
        foreach (Node child in control.GetChildren(true))
        {
            DisableHotkeys(child);
        }
    }
}
using Godot;
using System;

namespace Rusty.ISA;

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
        UndoRedo?.CreateAction($"Changed color {Title}: #{OldColor.ToHtml()} \u25B6 #{value.ToHtml()}");

        UndoRedo?.AddUndoProperty(ColorPickerButton, "color", OldColor);
        UndoRedo?.AddUndoMethod(new Callable(this, nameof(InvokeChangedEvent)));

        UndoRedo?.AddDoProperty(ColorPickerButton, "color", value);
        UndoRedo?.AddDoMethod(new Callable(this, nameof(InvokeChangedEvent)));

        UndoRedo?.CommitAction(true);
    }

    /* Private methods. */
    private void OnColorPickerOpened()
    {
        OldColor = ColorPickerButton.Color;
    }

    private void OnColorPickerClosed()
    {
        SetValue(ColorPickerButton.Color);
    }

    private void InvokeChangedEvent()
    {
        Changed?.Invoke(this);
    }
}
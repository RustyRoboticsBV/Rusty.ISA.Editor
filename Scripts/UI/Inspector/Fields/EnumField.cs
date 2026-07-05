using Godot;
using System;
using System.Security.Cryptography;

namespace Rusty.ActionGraph;

/// <summary>
/// An enum field.
/// </summary>
public partial class EnumField : HBoxContainer, IWidget, IValued<int>
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
            OptionButton.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo { get; set; }

    public string[] Choices => OptionsCache;
    public int Value => OptionButton.Selected;

    /* Private methods. */
    private Label Label { get; set; }
    private OptionButton OptionButton { get; set; }
    private string[] OptionsCache { get; set; } = [];
    private int OldSelected { get; set; }
    private bool Cancelled { get; set; }

    /* Public events. */
    public event Action<IWidget> Changed;

    /* Constructors. */
    public EnumField()
    {
        Label = new();
        AddChild(Label);
        TitleWidth = 160;

        OptionButton = new();
        OptionButton.Selected = -1;
        OptionButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        OptionButton.Pressed += OnDropdownOpened;
        OptionButton.ItemSelected += OnItemSelected;
        OptionButton.FocusExited += OnLostFocus;
        AddChild(OptionButton);

        WidgetRegistry.Add(this);
    }

    public EnumField(string[] choices) : this()
    {
        ChangeChoices(choices);
        if (choices.Length > 0)
            OptionButton.Selected = 0;
    }

    public EnumField(string[] choices, int selected) : this(choices)
    {
        OptionButton.Selected = selected;
    }
    
    ~EnumField()
    {
        WidgetRegistry.Remove(this);
    }

    /* Public methods. */
    public IWidget Copy()
    {
        EnumField field = new();
        field.SizeFlagsHorizontal = SizeFlagsHorizontal;
        field.SizeFlagsVertical = SizeFlagsVertical;
        field.Title = Title;
        field.TitleWidth = TitleWidth;
        field.Description = Description;
        field.ChangeChoices(Choices);
        field.OptionButton.Selected = Value;
        field.UndoRedo = UndoRedo;
        field.OldSelected = Value;
        return field;
    }

    public void ExpandFill(bool vertical = false)
    {
        SizeFlagsHorizontal = SizeFlags.ExpandFill;
        if (vertical)
            SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    public void SetValue(int value)
    {
        SetValue(OptionButton.Selected, value);
    }

    public void CancelFocus()
    {
        OptionButton.ReleaseFocus();
        OptionButton.GetPopup().Hide();
    }

    /* Private methods. */
    /// <summary>
    /// Silently change the choices, without changing the selected item index.
    /// </summary>
    private void ChangeChoices(string[] choices)
    {
        OptionsCache = choices;
        OptionButton.Clear();
        for (int i = 0; i < choices.Length; i++)
        {
            OptionButton.AddItem(choices[i]);
        }
    }

    private void SetValue(int from, int to)
    {
        if (from == to)
            return;

        string fromName = from >= 0 && from < Choices.Length ? Choices[from] : from.ToString();
        string toName = to >= 0 && to < Choices.Length ? Choices[to] : to.ToString();
        UndoRedo?.CreateAction($"Changed color {Title}: {fromName} \u25B6 {toName}");

        UndoRedo?.AddUndoMethod(new Callable(this, nameof(CancelAll)));
        UndoRedo?.AddUndoProperty(OptionButton, "selected", from);
        UndoRedo?.AddUndoMethod(new Callable(this, nameof(InvokeChangedEvent)));

        UndoRedo?.AddDoMethod(new Callable(this, nameof(CancelAll)));
        UndoRedo?.AddDoProperty(OptionButton, "selected", to);
        UndoRedo?.AddDoMethod(new Callable(this, nameof(InvokeChangedEvent)));

        UndoRedo?.CommitAction(false);

        OptionButton.Selected = to;
        InvokeChangedEvent();
    }

    private void CancelAll()
    {
        WidgetRegistry.ReleaseFocus();
    }

    private void OnLostFocus()
    {
        Cancelled = true;
        GetViewport().GuiGetFocusOwner()?.ReleaseFocus();
        Cancelled = false;
    }

    private void OnDropdownOpened()
    {
        OldSelected = OptionButton.Selected;
    }

    private void OnItemSelected(long index)
    {
        SetValue(OldSelected, (int)index);
    }

    private void InvokeChangedEvent()
    {
        Changed?.Invoke(this);
    }
}
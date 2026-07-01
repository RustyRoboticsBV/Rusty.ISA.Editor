using Godot;
using System;

namespace Rusty.ISA;

/// <summary>
/// A toggle field.
/// </summary>
[GlobalClass]
public partial class ToggleField : HBoxContainer, IWidget, IValued<bool>
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
            CheckBox.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo { get; set; }

    public bool Value
    {
        get => CheckBox.ButtonPressed;
        set => CheckBox.ButtonPressed = value;
    }

    /* Private methods. */
    private Label Label { get; set; }
    private CheckBox CheckBox { get; set; }

    /* Public events. */
    public event Action<IWidget> Changed;

    /* Constructors. */
    public ToggleField()
    {
        Label = new();
        AddChild(Label);
        TitleWidth = 160;

        CheckBox = new();
        AddChild(CheckBox);
        CheckBox.Pressed += OnChanged;
    }

    /* Public methods. */
    public IWidget Copy()
    {
        ToggleField field = new();
        field.SizeFlagsHorizontal = SizeFlagsHorizontal;
        field.SizeFlagsVertical = SizeFlagsVertical;
        field.Title = Title;
        field.TitleWidth = TitleWidth;
        field.Description = Description;
        field.Value = Value;
        return field;
    }

    public void ExpandFill(bool vertical = false)
    {
        SizeFlagsHorizontal = SizeFlags.ExpandFill;
        if (vertical)
            SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    public void SetValue(bool value)
    {
        Value = value;
    }

    public void CancelFocus() { }

    /* Private methods. */
    private void OnChanged()
    {
        Changed?.Invoke(this);
    }
}
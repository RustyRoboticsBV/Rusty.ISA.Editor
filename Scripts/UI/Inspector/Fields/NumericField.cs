using Godot;
using System;

namespace Rusty.ISA;

/// <summary>
/// A numeric field.
/// </summary>
public partial class NumericField : HBoxContainer, IWidget, IValued<double>
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
            SpinBox.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo { get; set; }

    public double Value
    {
        get => SpinBox.Value;
        set => SpinBox.Value = value;
    }

    public double Step
    {
        get => SpinBox.Step;
        set => SpinBox.Step = value;
    }
    public double MinValue
    {
        get => SpinBox.MinValue;
        set => SpinBox.MinValue = value;
    }
    public double MaxValue
    {
        get => SpinBox.MaxValue;
        set => SpinBox.MaxValue = value;
    }

    /* Private methods. */
    private Label Label { get; set; }
    private SpinBox SpinBox { get; set; }

    /* Public events. */
    public event Action<IWidget> Changed;

    /* Constructors. */
    public NumericField()
    {
        Label = new();
        AddChild(Label);
        TitleWidth = 160;

        SpinBox = new();
        AddChild(SpinBox);
        SpinBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        SpinBox.ValueChanged += OnChanged;
    }

    /* Public methods. */
    public IWidget Copy()
    {
        NumericField field = new();
        field.SizeFlagsHorizontal = SizeFlagsHorizontal;
        field.SizeFlagsVertical = SizeFlagsVertical;
        field.Title = Title;
        field.TitleWidth = TitleWidth;
        field.Description = Description;
        field.Value = Value;
        field.Step = Step;
        field.MinValue = MinValue;
        field.MaxValue = MaxValue;
        return field;
    }

    public void ExpandFill(bool vertical = false)
    {
        SizeFlagsHorizontal = SizeFlags.ExpandFill;
        if (vertical)
            SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    public void SetValue(double value)
    {
        Value = value;
    }

    /* Private methods. */
    private void OnChanged(double value)
    {
        Godot.GD.Print("FCUK NUMBER");
        Changed?.Invoke(this);
    }
}
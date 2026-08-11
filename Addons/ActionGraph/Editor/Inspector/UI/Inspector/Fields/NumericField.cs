using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A numeric field.
/// </summary>
public sealed partial class NumericField : HBoxContainer, IField<NumericField>
{
    /* Public properties. */
    public string TitleText
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    public int TitleWidth
    {
        get => (int)Label.CustomMinimumSize.X;
        set => Label.CustomMinimumSize = new(value, Label.CustomMinimumSize.Y);
    }
    public new string TooltipText
    {
        get => base.TooltipText;
        set
        {
            base.TooltipText = value;
            Label.TooltipText = value;
            SpinBox.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo
    {
        get => SpinBox.UndoRedo;
        set => SpinBox.UndoRedo = value;
    }

    public double Value => SpinBox.Value;

    public double Step => SpinBox.Step;
    public double MinValue => SpinBox.MinValue;
    public double MaxValue => SpinBox.MaxValue;

    /* Private methods. */
    private Label Label { get; set; }
    private SpinBox SpinBox { get; set; }

    /* Public events. */
    public event Action<NumericField> ValueChanged;

    /* Constructors. */
    public NumericField()
    {
        Label = new();
        AddChild(Label, false, InternalMode.Front);
        TitleWidth = 160;

        SpinBox = new();
        SpinBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        SpinBox.CustomMinimumSize = new(0, 31);
        SpinBox.ValueChanged += (value) => ValueChanged?.Invoke(this);
        AddChild(SpinBox, false, InternalMode.Front);
    }

    public NumericField(double value) : this()
    {
        SetValue(value);
    }

    public NumericField(string title, double value) : this(value)
    {
        TitleText = title;
    }

    public NumericField(double value, double min, double max, double step) : this()
    {
        SpinBox.MinValue = min;
        SpinBox.MaxValue = max;
        SpinBox.Step = step;
        SetValue(value);
    }

    public NumericField(string title, double value, double min, double max, double step) : this(value, min, max, step)
    {
        TitleText = title;
    }

    /* Public methods. */
    public NumericField DuplicateField()
    {
        NumericField field = Duplicate() as NumericField;
        field.TitleText = TitleText;
        field.TitleWidth = TitleWidth;
        field.TooltipText = TooltipText;
        field.UndoRedo = UndoRedo;
        field.SetMin(MinValue);
        field.SetMax(MaxValue);
        field.SetStep(Step);
        field.SetValue(SpinBox.Value);
        return field;
    }

    /// <summary>
    /// Set the minimum value.
    /// </summary>
    public void SetMin(double min) => SpinBox.MinValue = min;
    /// <summary>
    /// Set the maximum value.
    /// </summary>
    public void SetMax(double max) => SpinBox.MaxValue = max;
    /// <summary>
    /// Set the step interval.
    /// </summary>
    public void SetStep(double step) => SpinBox.Step = step;

    /// <summary>
    /// Change the value without recording it in undo/redo.
    /// </summary>
    public void SetValue(double value) => SpinBox.SetValue(value);
    /// <summary>
    /// Change the value and record it in undo/redo.
    /// </summary>
    public void CommitValue(double value) => SpinBox.CommitValue(value);
    /// <summary>
    /// If an edit is in progress, cancel it and revert to the last committed value.
    /// </summary>
    public void CancelValue() => SpinBox.CancelValue();
}
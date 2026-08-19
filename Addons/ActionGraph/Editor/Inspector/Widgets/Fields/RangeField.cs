using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A range slider field.
/// </summary>
internal sealed partial class RangeField : HBoxContainer, IField<RangeField>
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
            Slider.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo
    {
        get => Slider.UndoRedo;
        set => Slider.UndoRedo = value;
    }

    public double Value => Slider.Value;

    public double Step => Slider.Step;
    public double MinValue => Slider.MinValue;
    public double MaxValue => Slider.MaxValue;

    /* Private methods. */
    private Label Label { get; set; }
    private HSlider Slider { get; set; }
    private SpinBox SpinBox { get; set; }

    /* Public events. */
    public event Action StateChanged;

    /* Constructors. */
    public RangeField()
    {
        Label = new();
        AddChild(Label, false, InternalMode.Front);
        TitleWidth = 160;

        Slider = new();
        Slider.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Slider.CustomMinimumSize = new(0, 31);
        Slider.ValueChanged += (value) =>
        {
            if (SpinBox.Value != value)
                SpinBox.SetValue(value);
            StateChanged?.Invoke();
        };
        AddChild(Slider, false, InternalMode.Front);

        SpinBox = new();
        SpinBox.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
        SpinBox.CustomMinimumSize = new(0, 31);
        SpinBox.CommittedValue += (value) => Slider.CommitValue(value);
        AddChild(SpinBox, false, InternalMode.Front);
    }

    public RangeField(double value) : this() => SetValue(value);

    public RangeField(string title, double value) : this(value) => TitleText = title;

    public RangeField(double value, double min, double max, double step) : this()
    {
        Slider.MinValue = min;
        Slider.MaxValue = max;
        Slider.Step = step;
        SetValue(value);
    }

    public RangeField(string title, double value, double min, double max, double step) : this(value, min, max, step)
        => TitleText = title;

    /* Public methods. */
    public RangeField DuplicateWidget()
    {
        RangeField field = Duplicate() as RangeField;
        field.TitleText = TitleText;
        field.TitleWidth = TitleWidth;
        field.TooltipText = TooltipText;
        field.UndoRedo = UndoRedo;
        field.SetMin(MinValue);
        field.SetMax(MaxValue);
        field.SetStep(Step);
        field.SetValue(Slider.Value);
        return field;
    }

    /// <summary>
    /// Set the minimum value.
    /// </summary>
    public void SetMin(double min)
    {
        Slider.MinValue = min;
        SpinBox.MinValue = min;
    }
    /// <summary>
    /// Set the maximum value.
    /// </summary>
    public void SetMax(double max)
    {
        Slider.MaxValue = max;
        SpinBox.MaxValue = max;
    }
    /// <summary>
    /// Set the step interval.
    /// </summary>
    public void SetStep(double step)
    {
        Slider.Step = step;
        SpinBox.Step = step;
    }

    /// <summary>
    /// Change the value without recording it in undo/redo.
    /// </summary>
    public void SetValue(double value)
    {
        Slider.SetValue(value);
        SpinBox.SetValue(value);
    }
    /// <summary>
    /// Change the value and record it in undo/redo.
    /// </summary>
    public void CommitValue(double value)
    {
        SpinBox.SetValue(value);
        Slider.CommitValue(value);
    }
    /// <summary>
    /// If an edit is in progress, cancel it and revert to the last committed value.
    /// </summary>
    public void CancelValue()
    {
        Slider.CancelValue();
        SpinBox.CancelValue();
    }
}
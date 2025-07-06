using Godot;
using System.Diagnostics.Metrics;

namespace Rusty.ISA.Editor;

/// <summary>
/// A base class for sliders.
/// </summary>
public abstract partial class GenericSlider<T> : LabeledElement, RangeField<T>
{
    /* Public properties. */
    object Field.Value
    {
        get => Value;
        set => Value = (T)value;
    }
    public abstract T Value { get; set; }
    object RangeField.MinValue
    {
        get => MinValue;
        set => MinValue = (T)value;
    }
    public abstract T MinValue { get; set; }
    object RangeField.MaxValue
    {
        get => MaxValue;
        set => MaxValue = (T)value;
    }
    public abstract T MaxValue { get; set; }

    /* Protected properties. */
    protected HSlider Slider { get; set; }
    protected SpinBox SpinBox { get; set; }

    /* Constructors. */
    public GenericSlider()
    {
        // Add line edit.
        Slider = new();
        Slider.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Slider.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        Slider.MaxValue = 100;
        AddChild(Slider);
        Slider.ValueChanged += OnSliderChanged;

        // Add spin-box label.
        SpinBox = new();
        SpinBox.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
        SpinBox.CustomMinimumSize = new(128, 0);
        AddChild(SpinBox);
        SpinBox.ValueChanged += OnSpinBoxChanged;
    }

    /* Public methods. */
    public override void CopyFrom(IGuiElement other)
    {
        base.CopyFrom(other);

        if (other is GenericSlider<T> field)
        {
            Value = field.Value;
            MinValue = field.MinValue;
            MaxValue = field.MaxValue;
        }
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        base._Process(delta);

        SpinBox.MinValue = Slider.MinValue;
        SpinBox.MaxValue = Slider.MaxValue;
        SpinBox.Step = Slider.Step;
    }

    /* Private methods. */
    private void OnSliderChanged(double value)
    {
        SpinBox.Value = value;
    }

    private void OnSpinBoxChanged(double value)
    {
        Slider.Value = value;
    }
}
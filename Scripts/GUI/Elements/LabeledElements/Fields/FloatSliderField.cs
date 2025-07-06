using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A float slider field.
/// </summary>
public partial class FloatSliderField : LabeledElement, RangeField<float>
{
    /* Public properties. */
    object Field.Value
    {
        get => Value;
        set => Value = (float)value;
    }
    object RangeField.MinValue
    {
        get => MinValue;
        set => MinValue = (float)value;
    }
    object RangeField.MaxValue
    {
        get => MaxValue;
        set => MaxValue = (float)value;
    }
    public float Value
    {
        get => (float)Slider.Value;
        set => Slider.Value = value;
    }
    public float MinValue
    {
        get => (float)Slider.MinValue;
        set => Slider.MinValue = value;
    }
    public float MaxValue
    {
        get => (float)Slider.MaxValue;
        set => Slider.MaxValue = value;
    }

    /* Private properties. */
    private HSlider Slider { get; set; }
    private SpinBox Counter { get; set; }

    /* Public methods. */
    public override FloatSliderField Copy()
    {
        FloatSliderField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(FloatSliderField other)
    {
        CopyFrom(other as LabeledElement);
        Value = other.Value;
        MinValue = other.MinValue;
        MaxValue = other.MaxValue;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        base._Process(delta);

        Counter.MinValue = Slider.MinValue;
        Counter.MaxValue = Slider.MaxValue;
        Counter.Step = Slider.Step;
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add line edit.
        Slider = new();
        Slider.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Slider.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        Slider.Step = 0.00001;
        Slider.MaxValue = 100;
        AddChild(Slider);
        Slider.ValueChanged += OnSliderChanged;

        // Add counter label.
        Counter = new();
        Counter.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
        Counter.CustomMinimumSize = new(128, 0);
        AddChild(Counter);
        Counter.ValueChanged += OnSpinBoxChanged;
    }

    /* Private methods. */
    private void OnSliderChanged(double value)
    {
        Counter.Value = value;
    }

    private void OnSpinBoxChanged(double value)
    {
        Slider.Value = value;
    }
}
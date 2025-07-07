namespace Rusty.ISA.Editor;

/// <summary>
/// A float slider field.
/// </summary>
public partial class FloatSliderField : GenericSlider<float>
{
    /* Public properties. */
    public override float Value
    {
        get => (float)Slider.Value;
        set => Slider.Value = value;
    }
    public override float MinValue
    {
        get => (float)Slider.MinValue;
        set => Slider.MinValue = value;
    }
    public override float MaxValue
    {
        get => (float)Slider.MaxValue;
        set => Slider.MaxValue = value;
    }

    /* Constructors. */
    public FloatSliderField()
    {
        Slider.Step = 0.00001;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        FloatSliderField copy = new();
        copy.CopyFrom(this);
        return copy;
    }
}
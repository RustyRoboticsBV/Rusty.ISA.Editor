namespace Rusty.ISA.Editor;

/// <summary>
/// An integer slider field.
/// </summary>
public partial class IntSliderField : GenericSlider<int>
{
    /* Public properties. */
    public override int Value
    {
        get => (int)Slider.Value;
        set
        {
            Slider.Value = value;
            InvokeChanged();
        }
    }
    public override int MinValue
    {
        get => (int)Slider.MinValue;
        set
        {
            Slider.MinValue = value;
            InvokeChanged();
        }
    }
    public override int MaxValue
    {
        get => (int)Slider.MaxValue;
        set
        {
            Slider.MaxValue = value;
            InvokeChanged();
        }
    }

    /* Constructors. */
    public IntSliderField()
    {
        Slider.Step = 1;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        IntSliderField copy = new();
        copy.CopyFrom(this);
        return copy;
    }
}
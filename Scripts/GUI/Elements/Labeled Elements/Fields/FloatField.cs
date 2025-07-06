using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A float field.
/// </summary>
public partial class FloatField : GenericField<float, SpinBox>
{
    /* Public properties. */
    public override float Value
    {
        get => (float)Field.Value;
        set => Field.Value = value;
    }

    /* Constructors. */
    public FloatField() : base()
    {
        Field.Step = 0.00001;
        Field.MinValue = int.MinValue;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        FloatField copy = new();
        copy.CopyFrom(this);
        return copy;
    }
}
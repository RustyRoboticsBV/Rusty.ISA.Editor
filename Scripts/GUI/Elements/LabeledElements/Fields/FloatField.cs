using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A float spin-box field.
/// </summary>
public partial class FloatField : LabeledElement, Field<float>
{
    /* Public properties. */
    object Field.Value
    {
        get => Value;
        set => Value = (float)value;
    }
    public float Value
    {
        get => (float)Field.Value;
        set => Field.Value = value;
    }

    /* Private properties. */
    private SpinBox Field { get; set; }

    /* Public methods. */
    public override FloatField Copy()
    {
        FloatField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(FloatField other)
    {
        CopyFrom(other as LabeledElement);
        Value = other.Value;
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add spin-box.
        Field = new();
        Field.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Field.Step = 0.00001;
        Field.MinValue = int.MinValue;
        AddChild(Field);
    }
}
using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// An integer spin-box field.
/// </summary>
public partial class IntField : LabeledElement, Field<int>
{
    /* Public properties. */
    object Field.Value
    {
        get => Value;
        set => Value = (int)value;
    }
    public int Value
    {
        get => (int)Field.Value;
        set => Field.Value = value;
    }

    /* Private properties. */
    private SpinBox Field { get; set; }

    /* Public methods. */
    public override IntField Copy()
    {
        IntField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(IntField other)
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
        Field.Step = 1;
        Field.MinValue = int.MinValue;
        AddChild(Field);
    }
}
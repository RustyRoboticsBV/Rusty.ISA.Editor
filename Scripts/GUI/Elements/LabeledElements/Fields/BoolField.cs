using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A boolean button field.
/// </summary>
public partial class BoolField : LabeledElement, Field<bool>
{
    /* Public properties. */
    object Field.Value
    {
        get => Value;
        set => Value = (bool)value;
    }
    public bool Value
    {
        get => Field.ButtonPressed;
        set => Field.ButtonPressed = value;
    }

    /* Private properties. */
    private CheckButton Field { get; set; }

    /* Public methods. */
    public override BoolField Copy()
    {
        BoolField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(BoolField other)
    {
        CopyFrom(other as LabeledElement);
        Value = other.Value;
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add check button.
        Field = new();
        Field.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        AddChild(Field);
    }
}
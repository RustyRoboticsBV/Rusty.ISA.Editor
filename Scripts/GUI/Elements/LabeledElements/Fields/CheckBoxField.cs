using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A check-box field.
/// </summary>
public partial class CheckBoxField : LabeledElement, Field<bool>
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
    private CheckBox Field { get; set; }

    /* Public methods. */
    public override CheckBoxField Copy()
    {
        CheckBoxField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(CheckBoxField other)
    {
        CopyFrom(other as LabeledElement);
        Value = other.Value;
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add check-box.
        Field = new();
        Field.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Field);
    }
}
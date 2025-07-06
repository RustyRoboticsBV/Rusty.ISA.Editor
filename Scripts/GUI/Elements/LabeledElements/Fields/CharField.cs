using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A char line-edit field.
/// </summary>
public partial class CharField : LabeledElement, Field<char>
{
    /* Public properties. */
    object Field.Value
    {
        get => Value;
        set => Value = (char)value;
    }
    public char Value
    {
        get => Field.Text.Length > 0 ? Field.Text[0] : ' ';
        set => Field.Text = value.ToString();
    }

    /* Private properties. */
    private LineEdit Field { get; set; }

    /* Public methods. */
    public override CharField Copy()
    {
        CharField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(CharField other)
    {
        CopyFrom(other as LabeledElement);
        Value = other.Value;
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add line-edit.
        Field = new();
        Field.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Field.MaxLength = 1;
        AddChild(Field);
    }
}
using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A string line-edit field.
/// </summary>
public partial class ImageField : LabeledElement, Field<string>
{
    /* Public properties. */
    object Field.Value
    {
        get => Value;
        set => Value = (string)value;
    }
    public string Value
    {
        get => Field.Text;
        set => Field.Text = value;
    }

    /* Private properties. */
    private LineEdit Field { get; set; }

    /* Public methods. */
    public override ImageField Copy()
    {
        ImageField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(ImageField other)
    {
        CopyFrom(other as LabeledElement);
        Value = other.Value;
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add line edit.
        Field = new();
        Field.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Field);
    }
}
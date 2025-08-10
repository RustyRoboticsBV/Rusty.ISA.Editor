using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A single-line text field.
/// </summary>
public partial class LineField : LineEditField<string>
{
    /* Public properties. */
    public override string Value
    {
        get => Field.Text;
        set => Field.Text = value;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        LineField copy = new();
        copy.CopyFrom(this);
        return copy;
    }
}
using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A multi-line text field.
/// </summary>
public partial class MultilineField : GenericField<string, TextEdit>
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
        MultilineField copy = new();
        copy.CopyFrom(this);
        return copy;
    }
}
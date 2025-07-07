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

    /* Constructors. */
    public MultilineField() : base()
    {
        VBoxContainer vbox = new();
        vbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;

        while (GetChildCount() > 0)
        {
            Node child = GetChild(0);
            RemoveChild(child);
            vbox.AddChild(child);
        }
        AddChild(vbox);

        Field.CustomMinimumSize = new(0, 128);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        MultilineField copy = new();
        copy.CopyFrom(this);
        return copy;
    }
}
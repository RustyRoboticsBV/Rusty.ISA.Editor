using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A character field.
/// </summary>
public partial class CharField : GenericField<char, LineEdit>
{
    /* Public properties. */
    public override char Value
    {
        get => Field.Text.Length > 0 ? Field.Text[^1] : ' ';
        set => Field.Text = value.ToString();
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        CharField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Field.Text.Length > 1)
            Field.Text = Field.Text[^1].ToString();
    }
}
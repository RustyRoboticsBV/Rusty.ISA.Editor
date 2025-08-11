using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A boolean field.
/// </summary>
public partial class BoolField : GenericField<bool, CheckButton>
{
    /* Public properties. */
    public override bool Value
    {
        get => Field.ButtonPressed;
        set => Field.ButtonPressed = value;
    }

    /* Contructors. */
    public BoolField() : base()
    {
        Field.ButtonDown += OnPressed;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        BoolField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    /* Private methods. */
    private void OnPressed()
    {
        InvokeChanged();
    }
}
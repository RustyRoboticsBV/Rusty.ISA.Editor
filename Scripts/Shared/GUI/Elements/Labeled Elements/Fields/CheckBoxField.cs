using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A check-box field.
/// </summary>
public partial class CheckBoxField : GenericField<bool, CheckBox>
{
    /* Public properties. */
    public override bool Value
    {
        get => Field.ButtonPressed;
        set
        {
            Field.ButtonPressed = value;
            InvokeChanged();
        }
    }

    /* Contructors. */
    public CheckBoxField() : base()
    {
        Field.ButtonDown += OnPressed;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        CheckBoxField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    /* Private methods. */
    private void OnPressed()
    {
        InvokeChanged();
    }
}
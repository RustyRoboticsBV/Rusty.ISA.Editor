namespace Rusty.ISA.Editor;

/// <summary>
/// A integer field.
/// </summary>
public partial class IntField : SpinBoxField<int>
{
    /* Public properties. */
    public override int Value
    {
        get => (int)Field.Value;
        set
        {
            Field.Value = value;
            InvokeChanged();
        }
    }

    /* Constructors. */
    public IntField() : base()
    {
        Field.Step = 1;
        Field.MinValue = int.MinValue;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        IntField copy = new();
        copy.CopyFrom(this);
        return copy;
    }
}
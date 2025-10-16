using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A base class for fields.
/// </summary>
public abstract partial class GenericField<T, U> : LabeledElement, IField<T>
    where U : Control, new()
{
    /* Public properties. */
    object IField.Value
    {
        get => Value;
        set => Value = (T)value;
    }
    public abstract T Value { get; set; }
    public override string TooltipText
    {
        get => base.TooltipText;
        set
        {
            base.TooltipText = value;
            Field.TooltipText = value;
        }
    }

    /* Protected properties. */
    protected U Field { get; set; }

    /* Constructors. */
    public GenericField()
    {
        // Add field control.
        Field = new();
        Field.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Field);
    }

    /* Public methods. */
    public override void CopyFrom(IGuiElement other)
    {
        base.CopyFrom(other);

        if (other is GenericField<T, U> field)
            Value = field.Value;
    }
}
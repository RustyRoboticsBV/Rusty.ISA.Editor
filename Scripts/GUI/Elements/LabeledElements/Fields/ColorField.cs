using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A color field.
/// </summary>
public partial class ColorField : LabeledElement, Field<Color>
{
    /* Public properties. */
    object Field.Value
    {
        get => Value;
        set => Value = (Color)value;
    }
    public Color Value
    {
        get => Field.Color;
        set => Field.Color = value;
    }
    public new string TooltipText
    {
        get => base.TooltipText;
        set
        {
            base.TooltipText = value;
            Field.TooltipText = value;
        }
    }

    /* Private properties. */
    private ColorPickerButton Field { get; set; }

    /* Public methods. */
    public override ColorField Copy()
    {
        ColorField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(ColorField other)
    {
        CopyFrom(other as LabeledElement);
        Value = other.Value;
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add color picker button.
        Field = new();
        Field.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Field.CustomMinimumSize = new(0f, 32f);
        Field.Color = Colors.Red;
        AddChild(Field);
    }
}
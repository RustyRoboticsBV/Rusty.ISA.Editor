using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A color field.
/// </summary>
public partial class ColorField : GenericField<Color, ColorPickerButton>
{
    /* Public properties. */
    public override Color Value
    {
        get => Field.Color;
        set
        {
            Field.Color = value;
            InvokeChanged();
        }
    }

    /* Constructors. */
    public ColorField() : base()
    {
        Field.CustomMinimumSize = new(0f, 32f);
        Field.ColorChanged += OnColorChanged;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        ColorField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    /* Private methods. */
    private void OnColorChanged(Color color)
    {
        InvokeChanged();
    }
}
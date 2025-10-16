using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// An icon with a label.
/// </summary>
public partial class LabeledIcon : LabeledElement
{
    /* Public properties. */
    /// <summary>
    /// The displayed texture.
    /// </summary>
    public Texture2D Texture
    {
        get => TextureRect.Texture;
        set => TextureRect.Texture = value;
    }

    /* Protected properties. */
    protected TextureRect TextureRect { get; private set; }

    /* COnstructors. */
    public LabeledIcon() : base()
    {
        // Create texture rect.
        TextureRect = new();
        TextureRect.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        TextureRect.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        AddChild(TextureRect);

        // Add spacer.
        Control spacer = new();
        AddChild(spacer);

        // Move label back.
        MoveChild(Label, 2);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        LabeledIcon copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public override void CopyFrom(IGuiElement element)
    {
        base.CopyFrom(element);

        if (element is LabeledIcon icon)
            Texture = icon.Texture;
    }
}
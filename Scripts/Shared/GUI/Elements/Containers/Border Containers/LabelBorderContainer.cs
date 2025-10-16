namespace Rusty.ISA.Editor;

/// <summary>
/// A border container with a simple label in the header.
/// </summary>
public partial class LabelBorderContainer : BorderContainer
{
    /* Public properties. */
    public string LabelText
    {
        get => Label.LabelText;
        set => Label.LabelText = value;
    }

    /* Private properties. */
    private LabeledElement Label => GetFromHeader(0) as LabeledElement;

    /* Constructors. */
    public LabelBorderContainer() : base()
    {
        // Add label.
        LabeledElement label = new();
        label.LabelText = "Default text.";
        label.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        AddToHeader(label);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        LabelBorderContainer copy = new();
        copy.CopyFrom(this);
        return copy;
    }
}
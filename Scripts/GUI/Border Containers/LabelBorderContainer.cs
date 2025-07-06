namespace Rusty.ISA.Editor;

public partial class LabelBorderContainer : BorderContainer
{
    /* Public properties. */
    public string LabelText
    {
        get => Label.LabelText;
        set => Label.LabelText = value;
    }

    /* Private properties. */
    private LabeledElement Label { get; set; }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add label.
        Label = new();
        Label.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        AddToHeader(Label);
    }
}
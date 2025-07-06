namespace Rusty.ISA.Editor;

public partial class FoldoutBorderContainer : BorderContainer
{
    /* Public properties. */
    public string FoldoutText
    {
        get => Foldout.LabelText;
        set => Foldout.LabelText = value;
    }
    public bool FoldoutOpen
    {
        get => Foldout.IsOpen;
        set => Foldout.IsOpen = value;
    }

    /* Private properties. */
    private Foldout Foldout { get; set; }

    /* Godot overrides. */
    public override void _Ready()
    {
        _Process(0.0);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        HideContents = !Foldout.IsOpen;
        HideFooter = !Foldout.IsOpen;
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add fold-out.
        Foldout = new();
        Foldout.LabelText = FoldoutText;
        Foldout.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        AddToHeader(Foldout);
    }
}
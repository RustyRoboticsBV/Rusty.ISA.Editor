namespace Rusty.ISA.Editor;

public partial class FoldoutBorderContainer : BorderContainer
{
    /* Public properties. */
    public string FoldoutText
    {
        get => Foldout.LabelText;
        set => Foldout.LabelText = value;
    }
    public bool IsOpen
    {
        get => Foldout.IsOpen;
        set => Foldout.IsOpen = value;
    }

    /* Private properties. */
    private Foldout Foldout => GetFromHeader(0) as Foldout;

    /* Constructors. */
    public FoldoutBorderContainer() : base()
    {
        // Add fold-out.
        Foldout foldout = new();
        foldout.LabelText = "Foldout";
        foldout.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        AddToHeader(foldout);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        FoldoutBorderContainer copy = new();
        copy.CopyFrom(this);
        return copy;
    }

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
}
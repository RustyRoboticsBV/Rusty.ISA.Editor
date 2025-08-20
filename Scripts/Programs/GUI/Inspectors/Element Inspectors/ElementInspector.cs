namespace Rusty.ISA.Editor;

public partial class ElementInspector : Inspector
{
    /* Constants. */
    private const string Title = "title+icon";

    /* Public properties. */
    public PreviewInstance Preview { get; protected set; }

    /* Constructors. */
    public ElementInspector() : base()
    {
        // Change contents container.
        FoldoutBorderContainer foldout = new();
        foldout.IsOpen = true;
        ReplaceContainer(foldout);

        // Add title.
        LabeledIcon title = new();
        Add(Title, title);
    }

    public ElementInspector(InstructionSet set, string opcode) : this()
    {
        (ContentsContainer as FoldoutBorderContainer).FoldoutText = set[opcode].DisplayName;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        ElementInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }
}
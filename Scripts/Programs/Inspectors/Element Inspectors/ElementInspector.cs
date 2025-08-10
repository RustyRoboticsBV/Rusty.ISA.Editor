namespace Rusty.ISA.Editor;

public partial class ElementInspector : Inspector
{
    /* Constants. */
    private const string Title = "title+icon";

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

    public ElementInspector(InstructionSet set, InstructionDefinition definition) : this()
    {
        (ContentsContainer as FoldoutBorderContainer).FoldoutText = definition.DisplayName;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        ElementInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }
}
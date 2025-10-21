namespace Rusty.ISA.Editor;

public abstract partial class ElementInspector : ResourceInspector
{
    /* Constants. */
    private const string Title = "title+icon";

    /* Constructors. */
    public ElementInspector(InstructionSet set) : base(set)
    {
        // Change contents container.
        FoldoutBorderContainer foldout = new();
        foldout.IsOpen = true;
        ReplaceContainer(foldout);

        // Add title.
        LabeledIcon title = new();
        Add(Title, title);
    }

    public ElementInspector(InstructionSet set, string opcode) : this(set)
    {
        (ContentsContainer as FoldoutBorderContainer).FoldoutText = set[opcode].DisplayName;
    }
}
namespace Rusty.ISA.Editor;

public partial class CommentInspector : ElementInspector
{
    /* Public properties. */
    public InstructionDefinition Definition { get; private set; }

    /* Constructors. */
    public CommentInspector() : base() { }

    public CommentInspector(InstructionSet set, InstructionDefinition definition) : base(set, definition)
    {
        Definition = definition;

        // Text field.
        IGuiElement field = ParameterFieldFactory.Create(definition, BuiltIn.CommentText);
        Add(BuiltIn.CommentText, field);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        CommentInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public IField GetTextField()
    {
        return GetAt(BuiltIn.CommentText) as IField;
    }
}
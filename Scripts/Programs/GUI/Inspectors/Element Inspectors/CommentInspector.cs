namespace Rusty.ISA.Editor;

public partial class CommentInspector : ElementInspector
{
    /* Constructors. */
    public CommentInspector(InstructionSet set) : base(set, BuiltIn.CommentOpcode)
    {
        // Text field.
        ParameterInspector field = new(set, set[BuiltIn.CommentOpcode].GetParameter(BuiltIn.CommentText));
        Add(BuiltIn.CommentText, field);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        CommentInspector copy = new(InstructionSet);
        copy.CopyFrom(this);
        return copy;
    }

    public IField GetTextField()
    {
        return GetAt(BuiltIn.CommentText) as IField;
    }
}
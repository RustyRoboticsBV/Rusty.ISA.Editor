namespace Rusty.ISA.Editor;

/// <summary>
/// A comment-inspector pair.
/// </summary>
public sealed class LedgerComment : LedgerItem
{
    /* Public properties. */
    public new GraphComment Element => base.Element as GraphComment;
    public new CommentInspector Inspector => base.Inspector as CommentInspector;

    /* Constructors. */
    public LedgerComment(InstructionSet set, GraphComment element)
        : base(set, element, new CommentInspector(set))
    {
        InstructionDefinition definition = set[BuiltIn.CommentOpcode];
        element.BgColor = definition.EditorNode.MainColor;
        element.TextColor = definition.EditorNode.TextColor;

        OnInspectorChanged();
    }

    /* Protected methods. */
    protected override void OnInspectorChanged()
    {
        // Update comment text.
        Element.CommentText = Inspector.GetTextField().Value as string;
    }
}
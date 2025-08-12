using System;

namespace Rusty.ISA.Editor;

/// <summary>
/// A comment program unit.
/// </summary>
public sealed class CommentUnit : Unit
{
    /* Public properties. */
    public new GraphComment Element => base.Element as GraphComment;
    public new CommentInspector Inspector => base.Inspector as CommentInspector;

    /* Constructors. */
    public CommentUnit(InstructionSet set, string opcode, GraphComment element, Inspector inspector)
        : base(set, opcode, element, inspector) { }

    /* Public methods. */
    public override RootNode Compile()
    {
        // Compile comment.
        RootNode comment = CompilerNodeMaker.MakeRoot(Set, BuiltIn.CommentOpcode);
        comment.Data.SetArgument(BuiltIn.CommentText, Inspector.GetTextField().Value);
        comment.Data.SetArgument(BuiltIn.CommentX, Math.Round(Element.PositionOffset.X));
        comment.Data.SetArgument(BuiltIn.CommentY, Math.Round(Element.PositionOffset.Y));

        // Compile frame member.
        if (Element.Frame != null)
        {
            SubNode frameMember = CompilerNodeMaker.MakeSub(Set, BuiltIn.FrameMemberOpcode);
            frameMember.SetArgument(BuiltIn.FrameMemberID, Element.Frame.ID);
            comment.AddChild(frameMember);
        }

        // Compile end-of-group.
        comment.AddChild(CompilerNodeMaker.MakeSub(Set, BuiltIn.EndOfGroupOpcode));

        return comment;
    }

    /* Protected methods. */
    protected override void OnInspectorChanged()
    {
        base.OnInspectorChanged();

        // Update graph element.
        Element.CommentText = Inspector.GetTextField().Value as string;
    }
}
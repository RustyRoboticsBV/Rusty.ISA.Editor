namespace Rusty.ISA.Editor;

/// <summary>
/// A comment element compiler.
/// </summary>
public abstract class CommentCompiler : CompilerTool
{
    /* Public methods. */
    public static RootNode Compile(InstructionSet set, GraphComment element, CommentInspector inspector)
    {
        // Compile comment header.
        RootNode comment = MakeRoot(set, BuiltIn.CommentOpcode);
        comment.Data.SetArgument(BuiltIn.CommentText, inspector.GetTextField().Value);
        comment.Data.SetArgument(BuiltIn.CommentX, (int)element.PositionOffset.X);
        comment.Data.SetArgument(BuiltIn.CommentY, (int)element.PositionOffset.Y);

        // Compile frame member.
        if (element.Frame != null)
        {
            SubNode frameMember = MakeSub(set, BuiltIn.FrameMemberOpcode);
            frameMember.SetArgument(BuiltIn.FrameMemberID, element.Frame.ID);
            comment.AddChild(frameMember);
        }

        // Compile end-of-group.
        comment.AddChild(MakeSub(set, BuiltIn.EndOfGroupOpcode));

        return comment;
    }
}
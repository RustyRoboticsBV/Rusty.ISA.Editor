namespace Rusty.ISA.Editor;

/// <summary>
/// A frame element compiler.
/// </summary>
public static class FrameCompiler
{
    /* Public methods. */
    public static RootNode Compile(InstructionSet set, GraphFrame element, FrameInspector inspector)
    {
        // Compiler frame header.
        RootNode frame = CompilerNodeMaker.MakeRoot(set, BuiltIn.FrameOpcode);
        frame.SetArgument(BuiltIn.FrameID, element.ID);
        frame.SetArgument(BuiltIn.FrameX, (int)element.PositionOffset.X);
        frame.SetArgument(BuiltIn.FrameY, (int)element.PositionOffset.Y);
        frame.SetArgument(BuiltIn.FrameWidth, (int)element.Size.X);
        frame.SetArgument(BuiltIn.FrameHeight, (int)element.Size.Y);
        frame.SetArgument(BuiltIn.FrameTitle, inspector.GetTitleField().Value);
        frame.SetArgument(BuiltIn.FrameColor, inspector.GetColorField().Value);

        // Compile frame member.
        if (element.Frame != null)
        {
            SubNode frameMember = CompilerNodeMaker.MakeSub(set, BuiltIn.FrameMemberOpcode);
            frameMember.SetArgument(BuiltIn.FrameMemberID, element.Frame.ID);
            frame.AddChild(frameMember);
        }

        // Compile end-of-group.
        frame.AddChild(CompilerNodeMaker.MakeSub(set, BuiltIn.EndOfGroupOpcode));

        return frame;
    }
}
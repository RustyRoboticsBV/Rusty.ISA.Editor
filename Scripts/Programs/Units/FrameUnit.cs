using System.Xml.Linq;

namespace Rusty.ISA.Editor;

/// <summary>
/// A frame program unit.
/// </summary>
public sealed class FrameUnit : Unit
{
    /* Public properties. */
    public new GraphFrame Element => base.Element as GraphFrame;

    /* Constructors. */
    public FrameUnit(InstructionSet set, string opcode, GraphFrame element, Inspector inspector)
        : base(set, opcode, element, inspector) { }

    /* Public methods. */
    public override RootNode Compile()
    {
        RootNode frame = CompilerNodeMaker.MakeRoot(Set, BuiltIn.FrameOpcode);
        frame.SetArgument(BuiltIn.FrameID, Element.ID);
        frame.SetArgument(BuiltIn.FrameX, (int)Element.PositionOffset.X);
        frame.SetArgument(BuiltIn.FrameY, (int)Element.PositionOffset.Y);
        frame.SetArgument(BuiltIn.FrameWidth, (int)Element.Size.X);
        frame.SetArgument(BuiltIn.FrameHeight, (int)Element.Size.Y);
        frame.SetArgument(BuiltIn.FrameTitle, GetInspectorArg(BuiltIn.FrameTitle));
        frame.SetArgument(BuiltIn.FrameColor, GetInspectorArg(BuiltIn.FrameColor));

        // Compile frame member.
        if (Element.Frame != null)
        {
            SubNode frameMember = CompilerNodeMaker.MakeSub(Set, BuiltIn.FrameMemberOpcode);
            frameMember.SetArgument(BuiltIn.FrameMemberID, Element.Frame.ID);
            frame.AddChild(frameMember);
        }

        // Compile end-of-group.
        frame.AddChild(CompilerNodeMaker.MakeSub(Set, BuiltIn.EndOfGroupOpcode));

        return frame;
    }
}
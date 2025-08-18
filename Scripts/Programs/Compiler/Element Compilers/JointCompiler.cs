namespace Rusty.ISA.Editor;

/// <summary>
/// A joint element compiler.
/// </summary>
public abstract class JointCompiler : Compiler
{
    /* Public methods. */
    public static RootNode Compile(InstructionSet set, GraphJoint element, JointInspector inspector)
    {
        // Compile joint header.
        RootNode joint = MakeRoot(set, BuiltIn.JointOpcode);
        joint.SetArgument(BuiltIn.JointX, (int)element.PositionOffset.X);
        joint.SetArgument(BuiltIn.JointY, (int)element.PositionOffset.Y);

        // Compile frame member.
        if (element.Frame != null)
        {
            SubNode frameMember = MakeSub(set, BuiltIn.FrameMemberOpcode);
            frameMember.SetArgument(BuiltIn.FrameMemberID, element.Frame.ID);
            joint.AddChild(frameMember);
        }

        // Compile end-of-group.
        joint.AddChild(MakeSub(set, BuiltIn.EndOfGroupOpcode));
        return joint;
    }
}
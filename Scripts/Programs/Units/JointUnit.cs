namespace Rusty.ISA.Editor;

/// <summary>
/// A joint program unit.
/// </summary>
public sealed class JointUnit : Unit
{
    /* Public properties. */
    public new GraphJoint Element => base.Element as GraphJoint;

    /* Constructors. */
    public JointUnit(InstructionSet set, string opcode, GraphJoint element, Inspector inspector)
        : base(set, opcode, element, inspector) { }

    /* Public methods. */
    public override RootNode Compile()
    {
        // Compile joint header.
        RootNode joint = CompilerNodeMaker.MakeRoot(Set, BuiltIn.JointOpcode);

        // Compile frame member.
        if (Element.Frame != null)
        {
            SubNode frameMember = CompilerNodeMaker.MakeSub(Set, BuiltIn.FrameMemberOpcode);
            frameMember.SetArgument(BuiltIn.FrameMemberID, Element.Frame.ID);
            joint.AddChild(frameMember);
        }

        // Compile end-of-group.
        joint.AddChild(CompilerNodeMaker.MakeSub(Set, BuiltIn.EndOfGroupOpcode));
        return joint;
    }
}
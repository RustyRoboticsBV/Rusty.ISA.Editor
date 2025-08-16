namespace Rusty.ISA.Editor;

public partial class JointInspector : ElementInspector
{
    /* Constructors. */
    public JointInspector() : base() { }

    public JointInspector(InstructionSet set) : base(set, BuiltIn.JointOpcode) { }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        JointInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }
}
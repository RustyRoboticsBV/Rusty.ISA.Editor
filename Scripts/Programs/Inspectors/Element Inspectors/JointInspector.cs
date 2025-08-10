namespace Rusty.ISA.Editor;

public partial class JointInspector : ElementInspector
{
    /* Public properties. */
    public InstructionDefinition Definition { get; private set; }

    /* Constructors. */
    public JointInspector() : base() { }

    public JointInspector(InstructionSet set, InstructionDefinition definition) : base(set, definition)
    {
        Definition = definition;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        JointInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }
}
namespace Rusty.ISA.Editor;

/// <summary>
/// A joint-inspector pair.
/// </summary>
public sealed class LedgerJoint : LedgerItem
{
    /* Public properties. */
    public new GraphJoint Element => base.Element as GraphJoint;
    public new JointInspector Inspector => base.Inspector as JointInspector;

    /* Constructors. */
    public LedgerJoint(InstructionSet set, GraphJoint element)
        : base(set, element, new JointInspector(set)) { }

    /* Protected methods. */
    protected override void OnInspectorChanged() { }
}
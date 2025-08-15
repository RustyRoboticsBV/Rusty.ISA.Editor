using Godot;
namespace Rusty.ISA.Editor;

public class ProgramUnits
{
    /* Public properties. */
    public InstructionSet InstructionSet { get; set; }
    public GraphEdit GraphEdit { get; set; }
    public DualDict<IGraphElement, Inspector, Unit> Contents { get; } = new();

    /* Constructors. */
    public ProgramUnits(InstructionSet set, GraphEdit graphEdit)
    {
        InstructionSet = set;
        GraphEdit = graphEdit;
    }

    /* Public methods. */
    public void Clear()
    {
        // Delete all graph elements.
        foreach (GraphNode node in GraphEdit.Nodes)
        {
            GraphEdit.RemoveChild(node);
        }
        GraphEdit.Nodes.Clear();
        foreach (GraphJoint joint in GraphEdit.Joints)
        {
            GraphEdit.RemoveChild(joint);
        }
        GraphEdit.Joints.Clear();
        foreach (GraphComment comment in GraphEdit.Comments)
        {
            GraphEdit.RemoveChild(comment);
        }
        GraphEdit.Comments.Clear();
        foreach (GraphFrame frame in GraphEdit.Frames)
        {
            GraphEdit.RemoveChild(frame);
        }
        GraphEdit.Frames.Clear();

        // Remove inspectors.
        foreach (Unit unit in Contents)
        {
            unit.Inspector.GetParent()?.RemoveChild(unit.Inspector);
        }

        // Delete units.
        Contents.Clear();
    }
}
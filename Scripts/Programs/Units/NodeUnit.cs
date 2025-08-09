namespace Rusty.ISA.Editor;

/// <summary>
/// A node program unit.
/// </summary>
public sealed class NodeUnit : Unit
{
    /* Public properties. */
    public new GraphNode Element => base.Element as GraphNode;

    /* Constructors. */
    public NodeUnit(InstructionSet set, string opcode, GraphNode element, Inspector inspector)
        : base(set, opcode, element, inspector) { }

    /* Public methods. */
    public override RootNode Compile()
    {
        RootNode node = CompilerNodeMaker.MakeRoot(Set, BuiltIn.NodeOpcode);
        node.SetArgument(BuiltIn.NodeX, (int)Element.PositionOffset.X);
        node.SetArgument(BuiltIn.NodeY, (int)Element.PositionOffset.Y);

        // Compile begin.
        if (Inspector.GetAt(ElementInspectorFactory.StartPoint) is ToggleTextField startPoint && startPoint.Checked)
        {
            SubNode begin = CompilerNodeMaker.MakeSub(Set, BuiltIn.BeginOpcode);
            begin.SetArgument(BuiltIn.BeginName, startPoint.Value);
            node.AddChild(begin);
        }

        // Compile frame member.
        if (Element.Frame != null)
        {
            SubNode frame = CompilerNodeMaker.MakeSub(Set, BuiltIn.FrameMemberOpcode);
            node.AddChild(frame);
        }

        // Compile end-of-group.
        node.AddChild(CompilerNodeMaker.MakeSub(Set, BuiltIn.EndOfGroupOpcode));

        return node;
    }
}
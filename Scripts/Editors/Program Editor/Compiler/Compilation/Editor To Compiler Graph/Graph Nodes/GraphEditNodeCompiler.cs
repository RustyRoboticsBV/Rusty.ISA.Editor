using Godot;
using Rusty.ISA;
using Rusty.Graphs;

namespace Rusty.ISA.Editor.Programs.Compiler
{
    public abstract class GraphEditNodeCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Take a graph edit node, and convert it and its inspector data to a compiler node.
        /// Inputs and outputs are NOT added by this method!
        /// </summary>
        public static CompilerNode Compile(GraphInstruction graphNode)
        {
            InstructionSet set = graphNode.InstructionSet;
            int x = Mathf.RoundToInt(graphNode.PositionOffset.X);
            int y = Mathf.RoundToInt(graphNode.PositionOffset.Y);

            // Create NOD node.
            CompilerNode node = CompilerNodeMaker.GetNode(set, x.ToString(), y.ToString());

            // Add BEG sub-node.
            if (graphNode.Inspector.LabelName != "")
                node.AddChild(CompilerNodeMaker.GetBegin(set, graphNode.Inspector.LabelName));

            // Compile instruction inspector hierarchy and add it as a sub-node.
            node.AddChild(InspectorCompiler.Compile(graphNode.Inspector));

            // Add EOG sub-node.
            node.AddChild(CompilerNodeMaker.GetEndOfGroup(set));

            // Return finished node.
            return node;
        }
    }
}
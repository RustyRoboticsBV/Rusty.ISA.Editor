using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    public class NodeInstructionCompiler : InstructionCompiler
    {
        /* Public methods. */
        public static RootNode<NodeData> Compile(NodeInstructionInspector inspector)
        {
            InstructionSet set = inspector.InstructionSet;

            // Editor node.
            RootNode<NodeData> editorNode = GetNode(inspector);

            // Label.
            if (inspector.LabelName != "")
            {
                SubNode<NodeData> label = GetLabel(inspector);
                editorNode.AddChild(label);
            }

            // Instruction.
            SubNode<NodeData> instruction = GetInstruction(inspector);
            editorNode.AddChild(instruction);

            return editorNode;
        }
    }
}
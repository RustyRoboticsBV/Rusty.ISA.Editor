using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    public abstract class GraphNodeCompiler : Compiler
    {
        /* Public methods. */
        public static RootNode<NodeData> GetInstruction(CutsceneGraphNode graphNode)
        {
            // Add node instruction.
            RootNode<NodeData> node = GetNode(graphNode);

            // Add (optional) label instruction.
            if (graphNode.NodeInspector.LabelName != "")
                node.AddChild(GetLabel(graphNode.NodeInspector));

            // Add inspector instruction.
            node.AddChild(InstructionCompiler.Compile(graphNode.NodeInspector));

            return node;
        }

        /* Private methods. */
        private static RootNode<NodeData> GetNode(CutsceneGraphNode graphNode)
        {
            InstructionSet set = graphNode.InstructionSet;
            InstructionDefinition definition = set[BuiltIn.NodeOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.NodeXId)] = ((int)graphNode.Position.X).ToString();
            instance.Arguments[definition.GetParameterIndex(BuiltIn.NodeYId)] = ((int)graphNode.Position.Y).ToString();

            return new(instance.ToString(), new(set, definition, instance));
        }

        private static SubNode<NodeData> GetLabel(NodeInstructionInspector inspector)
        {
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = set[BuiltIn.LabelOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.LabelNameId)] = inspector.LabelName;

            return new(instance.ToString(), new(set, definition, instance));
        }
    }
}
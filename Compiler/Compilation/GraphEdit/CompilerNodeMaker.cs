using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    public abstract class CompilerNodeMaker : Compiler
    {
        /* Public methods. */
        public static RootNode<NodeData> Create(InstructionSet set, string x, string y, string start, SubNode<NodeData> subNode)
        {
            // Add node instruction.
            RootNode<NodeData> node = GetNode(set, x, y);

            // Add label instruction.
            node.AddChild(GetLabel(set));

            // Add (optional) start instruction.
            if (start != "")
                node.AddChild(GetStart(set, start));

            // Add inspector instruction.
            if (subNode != null)
                node.AddChild(subNode);

            // Add end-of-block instruction.
            node.AddChild(GetEndOfBlock(set));

            return node;
        }

        public static RootNode<NodeData> Create(InstructionSet set, int x, int y, string start, SubNode<NodeData> subNode)
        {
            return Create(set, x.ToString(), y.ToString(), start, subNode);
        }

        public static RootNode<NodeData> Create(InstructionSet set, string opcode)
        {
            InstructionDefinition definition = set[opcode];
            InstructionInstance instance = new(definition);
            return Create(set, "-", "-", "", new SubNode<NodeData>(new NodeData(set, definition, instance)));
        }

        /* Private methods. */
        /// <summary>
        /// Create a NOD instruction root-node.
        /// </summary>
        private static RootNode<NodeData> GetNode(InstructionSet set, string x, string y)
        {
            InstructionDefinition definition = set[BuiltIn.NodeOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.NodeXId)] = x;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.NodeYId)] = y;

            return new(instance.ToString(), new(set, definition, instance));
        }

        /// <summary>
        /// Create a STA instruction sub-node.
        /// </summary>
        private static SubNode<NodeData> GetStart(InstructionSet set, string name)
        {
            InstructionDefinition definition = set[BuiltIn.StartOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.StartNameId)] = name;

            return new(instance.ToString(), new(set, definition, instance));
        }

        /// <summary>
        /// Create a LBL instruction sub-node.
        /// </summary>
        private static SubNode<NodeData> GetLabel(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.LabelOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.LabelNameId)] = "MISSING_LABEL";

            return new(instance.ToString(), new(set, definition, instance));
        }
    }
}
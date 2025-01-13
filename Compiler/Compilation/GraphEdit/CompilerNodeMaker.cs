using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// An utility for creating compiler nodes.
    /// </summary>
    public abstract class CompilerNodeMaker : Compiler
    {
        /* Public methods. */
        /// <summary>
        /// Create a compiler node hierarchy, starting with a NOD instruction.
        /// </summary>
        public static CompilerNode CreateHierarchy(InstructionSet set, string x, string y, string name, SubNode<NodeData> subNode)
        {
            // Add node instruction.
            CompilerNode node = GetNode(set, x, y);

            // Add (optional) start instruction.
            if (name != "")
                node.AddChild(GetStart(set, name));

            // Add inspector instruction.
            if (subNode != null)
                node.AddChild(subNode);

            // Add end-of-block instruction.
            node.AddChild(GetEndOfBlock(set));

            return node;
        }

        /// <summary>
        /// Create a compiler node hierarchy, starting with a NOD instruction.
        /// </summary>
        public static CompilerNode CreateHierarchy(InstructionSet set, int x, int y, string start, SubNode<NodeData> subNode)
        {
            return CreateHierarchy(set, x.ToString(), y.ToString(), start, subNode);
        }

        /// <summary>
        /// Create a compiler node hierarchy, starting a NOD instruction, with some other instruction as their sub-node.
        /// </summary>
        public static CompilerNode CreateHierarchy(InstructionSet set, string opcode)
        {
            InstructionDefinition definition = set[opcode];
            InstructionInstance instance = new(definition);

            // Add pre-instruction block starter node.
            SubNode<NodeData> pre = GetPreInstructionBlock(set);

            // Create instruction node.
            pre.AddChild(GetInstruction(set, instance));

            // Create block end node.
            pre.AddChild(GetEndOfBlock(set));

            // Create node hierarchy.
            return CreateHierarchy(set, "-", "-", "", pre);
        }


        /// <summary>
        /// Create a NOD instruction root-node.
        /// </summary>
        public static CompilerNode GetNode(InstructionSet set, string x, string y)
        {
            InstructionDefinition definition = set[BuiltIn.NodeOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.NodeXId)] = x;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.NodeYId)] = y;

            return new CompilerNode(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a LBL instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetLabel(InstructionSet set, string value)
        {
            InstructionDefinition definition = set[BuiltIn.LabelOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.LabelNameId)] = value;

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a STA instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetStart(InstructionSet set, string name)
        {
            InstructionDefinition definition = set[BuiltIn.StartOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.StartNameId)] = name;

            return new (new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a PRE instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetPreInstructionBlock(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.InstructorInspectorOpcode];
            InstructionInstance instance = new(definition);

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a OPT instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetOptionRule(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.OptionRuleOpcode];
            InstructionInstance instance = new(definition);

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a CHO instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetChoiceRule(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.ChoiceRuleOpcode];
            InstructionInstance instance = new(definition);

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a TPL instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetTupleRule(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.TupleRuleOpcode];
            InstructionInstance instance = new(definition);

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a LST instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetListRule(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.ListRuleOpcode];
            InstructionInstance instance = new(definition);

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a EOB instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetEndOfBlock(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.EndOfBlockOpcode];
            InstructionInstance instance = new(definition);

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create an instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetInstruction(InstructionSet set, InstructionInstance instance)
        {
            return new SubNode<NodeData>(new NodeData(set, set[instance.Opcode], instance));
        }
    }
}
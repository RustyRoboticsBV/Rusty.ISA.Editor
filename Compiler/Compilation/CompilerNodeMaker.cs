using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// An utility for creating compiler nodes.
    /// </summary>
    public abstract class CompilerNodeMaker
    {
        /// <summary>
        /// Create a NOD instruction root-node.
        /// </summary>
        public static CompilerNode GetNode(InstructionSet set, string x, string y)
        {
            InstructionDefinition definition = set[BuiltIn.NodeOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.NodeXID)] = x;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.NodeYID)] = y;

            return new CompilerNode(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a BEG instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetBegin(InstructionSet set, string name)
        {
            InstructionDefinition definition = set[BuiltIn.BeginOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.BeginNameID)] = name;

            return new(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a END instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetEnd(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.EndOpcode];
            InstructionInstance instance = new(definition);

            return new(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a LAB instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetLabel(InstructionSet set, string value)
        {
            InstructionDefinition definition = set[BuiltIn.LabelOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.LabelNameID)] = value;

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a GTO instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetGoto(InstructionSet set, string targetLabel)
        {
            InstructionDefinition definition = set[BuiltIn.GotoOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.GotoTargetLabelID)] = targetLabel;

            return new(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create an INS instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetInspector(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.InspectorOpcode];
            InstructionInstance instance = new(definition);

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a PRE instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetPreInstructions(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.PreInstructionOpcode];
            InstructionInstance instance = new(definition);

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a PST instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetPostInstructions(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.PostInstructionOpcode];
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
        public static SubNode<NodeData> GetEndOfGroup(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.EndOfGroupOpcode];
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
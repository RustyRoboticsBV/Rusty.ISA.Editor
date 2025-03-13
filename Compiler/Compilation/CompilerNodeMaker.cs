using Godot;
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
        /// Create a CMT instruction root-node.
        /// </summary>
        public static CompilerNode GetComment(InstructionSet set, string x, string y, string text)
        {
            InstructionDefinition definition = set[BuiltIn.CommentOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.CommentX)] = x;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.CommentY)] = y;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.CommentText)] = text;

            return new CompilerNode(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a FRM instruction root-node.
        /// </summary>
        public static CompilerNode GetFrame(InstructionSet set, string id, string x, string y, string width, string height,
            string title, Color color)
        {
            InstructionDefinition definition = set[BuiltIn.FrameOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.FrameID)] = id;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.FrameX)] = x;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.FrameY)] = y;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.FrameWidth)] = width;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.FrameHeight)] = height;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.FrameTitle)] = title;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.FrameColor)] = '#' + color.ToHtml();

            return new CompilerNode(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a NOD instruction root-node.
        /// </summary>
        public static CompilerNode GetNode(InstructionSet set, string x, string y)
        {
            InstructionDefinition definition = set[BuiltIn.NodeOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.NodeX)] = x;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.NodeY)] = y;

            return new CompilerNode(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a BEG instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetBegin(InstructionSet set, string name)
        {
            InstructionDefinition definition = set[BuiltIn.BeginOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.BeginName)] = name;

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

            instance.Arguments[definition.GetParameterIndex(BuiltIn.LabelName)] = value;

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a GTO instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetGoto(InstructionSet set, string targetLabel)
        {
            InstructionDefinition definition = set[BuiltIn.GotoOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.GotoTargetLabel)] = targetLabel;

            return new(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a MBR instruction sub-node.
        /// </summary>
        public static CompilerNode GetFrameMember(InstructionSet set, string id)
        {
            InstructionDefinition definition = set[BuiltIn.FrameMemberOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.FrameMemberID)] = id;

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
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

        /// <summary>
        /// Create a SET instruction node.
        /// </summary>
        public static CompilerNode GetSet(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.MetadataOpcode];
            InstructionInstance instance = new(definition);

            return new CompilerNode(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a DEF instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetDefinition(InstructionSet set, string opcode)
        {
            InstructionDefinition definition = set[BuiltIn.DefinitionOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.DefinitionOpcodeParameter)] = opcode;

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a PAR instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetParameter(InstructionSet set, string type, string id)
        {
            InstructionDefinition definition = set[BuiltIn.ParameterOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.ParameterType)] = type;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.ParameterID)] = id;

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a RUL instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetCompileRule(InstructionSet set, string type, string id)
        {
            InstructionDefinition definition = set[BuiltIn.CompileRuleOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.CompileRuleType)] = type;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.CompileRuleID)] = id;

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }

        /// <summary>
        /// Create a REF instruction sub-node.
        /// </summary>
        public static SubNode<NodeData> GetReference(InstructionSet set, string type, string id)
        {
            InstructionDefinition definition = set[BuiltIn.ReferenceOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.ReferenceOpcodeParameter)] = type;
            instance.Arguments[definition.GetParameterIndex(BuiltIn.ReferenceID)] = id;

            return new SubNode<NodeData>(new NodeData(set, definition, instance));
        }
    }
}
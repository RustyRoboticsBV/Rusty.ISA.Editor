using System.Collections.Generic;
using Rusty.Cutscenes;
using Rusty.Graphs;
using Rusty.CutsceneImporter.InstructionDefinitions;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// An instruction set metadata compiler.
    /// </summary>
    public static class MetadataCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Create an instruction set metadata hierarchy.
        /// </summary>
        public static CompilerNode Compile(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.NodeOpcode];

            // Add SET node.
            CompilerNode node = CompilerNodeMaker.GetMetadata(set);

            // Add DEF nodes.
            for (int i = 0; i < set.Count; i++)
            {
                node.AddChild(GetInstructionDefinition(set, set[i]));
            }

            // Add EOG node.
            node.AddChild(CompilerNodeMaker.GetEndOfGroup(set));

            return node;
        }

        /* Private methods. */
        /// <summary>
        /// Create an instruction definition hierarchy.
        /// </summary>
        private static SubNode<NodeData> GetInstructionDefinition(InstructionSet set, InstructionDefinition definition)
        {
            // Add DEF node.
            SubNode<NodeData> node = CompilerNodeMaker.GetDefinition(set, definition.Opcode);

            // Add PAR nodes.
            for (int i = 0; i < definition.Parameters.Length; i++)
            {
                node.AddChild(CreateParameter(set, definition.Parameters[i]));
            }

            // Add PRE node.
            if (definition.PreInstructions.Length > 0)
            {
                SubNode<NodeData> pre = CompilerNodeMaker.GetPreInstructions(set);
                foreach (CompileRule preRule in definition.PreInstructions)
                {
                    pre.AddChild(CreateCompileRule(set, preRule));
                }
                pre.AddChild(CompilerNodeMaker.GetEndOfGroup(set));
                node.AddChild(pre);
            }

            // Add PST node.
            if (definition.PostInstructions.Length > 0)
            {
                SubNode<NodeData> post = CompilerNodeMaker.GetPostInstructions(set);
                foreach (CompileRule postRule in definition.PostInstructions)
                {
                    post.AddChild(CreateCompileRule(set, postRule));
                }
                post.AddChild(CompilerNodeMaker.GetEndOfGroup(set));
                node.AddChild(post);
            }

            // Add EOG node.
            node.AddChild(CompilerNodeMaker.GetEndOfGroup(set));

            return node;
        }

        /// <summary>
        /// Create a parameter definition hierarchy.
        /// </summary>
        private static SubNode<NodeData> CreateParameter(InstructionSet set, Parameter parameter)
        {
            InstructionDefinition definition = set[BuiltIn.ParameterOpcode];

            string type = "";
            switch (parameter)
            {
                case BoolParameter:
                    type = Keywords.BoolParameter;
                    break;
                case IntParameter:
                    type = Keywords.IntParameter;
                    break;
                case IntSliderParameter:
                    type = Keywords.IntSliderParameter;
                    break;
                case FloatParameter:
                    type = Keywords.FloatParameter;
                    break;
                case FloatSliderParameter:
                    type = Keywords.FloatSliderParameter;
                    break;
                case CharParameter:
                    type = Keywords.CharParameter;
                    break;
                case TextParameter:
                    type = Keywords.TextParameter;
                    break;
                case MultilineParameter:
                    type = Keywords.MultilineParameter;
                    break;
                case ColorParameter:
                    type = Keywords.ColorParameter;
                    break;
                case OutputParameter:
                    type = Keywords.OutputParameter;
                    break;
            }

            return CompilerNodeMaker.GetParameter(set, type, parameter.ID);
        }

        /// <summary>
        /// Create a compile rule definition hierarchy.
        /// </summary>
        private static SubNode<NodeData> CreateCompileRule(InstructionSet set, CompileRule rule)
        {
            InstructionDefinition definition = set[BuiltIn.CompileRuleOpcode];
            InstructionInstance instance = new(definition);

            // Get some data from the rule.
            string type = "";
            List<CompileRule> childRules = new();
            switch (rule)
            {
                case InstructionRule instructionRule:
                    return CompilerNodeMaker.GetReference(set, instructionRule.Opcode, instructionRule.ID);

                case OptionRule optionRule:
                    type = Keywords.OptionRule;
                    childRules.Add(optionRule.Type);
                    break;
                case ChoiceRule choiceRule:
                    type = Keywords.ChoiceRule;
                    foreach (CompileRule choice in choiceRule.Types)
                    {
                        childRules.Add(choice);
                    }
                    break;
                case TupleRule tupleRule:
                    type = Keywords.TupleRule;
                    foreach (CompileRule element in tupleRule.Types)
                    {
                        childRules.Add(element);
                    }
                    break;
                case ListRule listRule:
                    type = Keywords.ListRule;
                    childRules.Add(listRule.Type);
                    break;
            }

            // Add RUL node.
            SubNode<NodeData> node = CompilerNodeMaker.GetCompileRule(set, type, rule.ID);

            // Add child rule nodes.
            foreach (CompileRule childRule in childRules)
            {
                node.AddChild(CreateCompileRule(set, childRule));
            }

            // Add EOG node.
            node.AddChild(CompilerNodeMaker.GetEndOfGroup(set));

            return node;
        }
    }
}
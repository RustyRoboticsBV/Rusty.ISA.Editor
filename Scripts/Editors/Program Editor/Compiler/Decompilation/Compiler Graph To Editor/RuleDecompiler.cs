using System;
using Rusty.Graphs;

namespace Rusty.ISA.ProgramEditor.Compiler
{
    /// <summary>
    /// A base class for rule node decompilers.
    /// </summary>
    public abstract class RuleDecompiler
    {
        /// <summary>
        /// Apply a compile rule node to a target inspector.
        /// </summary>
        public static void ApplyRule(Node<NodeData> rule, Inspector ruleInspector)
        {
            if (rule.Data.GetOpcode() == BuiltIn.InspectorOpcode && ruleInspector is InstructionRuleInspector instruction)
                InspectorDecompiler.Apply(rule, instruction.TargetInstruction);

            else if (rule.Data.GetOpcode() == BuiltIn.OptionRuleOpcode && ruleInspector is OptionRuleInspector option)
                OptionRuleDecompiler.Apply(rule, option);

            else if (rule.Data.GetOpcode() == BuiltIn.ChoiceRuleOpcode && ruleInspector is ChoiceRuleInspector choice)
                ChoiceRuleDecompiler.Apply(rule, choice);

            else if (rule.Data.GetOpcode() == BuiltIn.TupleRuleOpcode && ruleInspector is TupleRuleInspector tuple)
                TupleRuleDecompiler.Apply(rule, tuple);

            else if (rule.Data.GetOpcode() == BuiltIn.ListRuleOpcode && ruleInspector is ListRuleInspector list)
                ListRuleDecompiler.Apply(rule, list);

            else
            {
                throw new Exception($"Tried to apply an editor node with opcode '{rule.Data.GetOpcode()}' to an inspector of "
                    + $"type '{ruleInspector.GetType().Name}', but this is not allowed!");
            }
        }
    }
}
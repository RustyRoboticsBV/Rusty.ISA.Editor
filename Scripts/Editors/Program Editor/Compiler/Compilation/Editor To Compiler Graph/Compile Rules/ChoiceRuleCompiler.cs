﻿using Rusty.ISA;
using Rusty.Graphs;

namespace Rusty.ISA.ProgramEditor.Compiler
{
    /// <summary>
    /// A compiler that converts a choice rule inspector into a graph node.
    /// </summary>
    public abstract class ChoiceRuleCompiler : RuleCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile a choice rule inspector into a compiler node hierarchy.
        /// </summary>
        public static SubNode<NodeData> Compile(ChoiceRuleInspector inspector)
        {
            // Main rule.
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = set[BuiltIn.ChoiceRuleOpcode];
            InstructionInstance instance = new(definition);

            SubNode<NodeData> choice = CompilerNodeMaker.GetChoiceRule(set);
            choice.Data.SetArgument(BuiltIn.ChoiceRuleSelected, inspector.Selected.ToString());

            // Child rules.
            Inspector[] childRules = inspector.GetActiveSubInspectors();
            foreach (Inspector childRule in childRules)
            {
                choice.AddChild(Compile(childRule));
            }

            // End of rule.
            choice.AddChild(CompilerNodeMaker.GetEndOfGroup(inspector.InstructionSet));

            return choice;
        }
    }
}
using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// A compiler that converts an option rule inspector into a graph node.
    /// </summary>
    public abstract class OptionRuleCompiler : RuleCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile a option rule inspector into a compiler node hierarchy.
        /// </summary>
        public static SubNode<NodeData> Compile(OptionRuleInspector inspector)
        {
            // Main rule.
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = set[BuiltIn.OptionRuleOpcode];
            InstructionInstance instance = new(definition);

            SubNode<NodeData> option = CompilerNodeMaker.GetOptionRule(set);

            // Child rules.
            Inspector[] childRules = inspector.GetActiveSubInspectors();
            foreach (Inspector childRule in childRules)
            {
                option.AddChild(Compile(childRule));
            }

            // End of rule.
            option.AddChild(CompilerNodeMaker.GetEndOfBlock(inspector.InstructionSet));

            return option;
        }
    }
}
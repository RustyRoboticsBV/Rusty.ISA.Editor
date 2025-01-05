using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    public abstract class OptionRuleCompiler : RuleCompiler
    {
        /* Public methods. */
        public static SubNode<NodeData> Compile(OptionRuleInspector inspector)
        {
            // Main rule.
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = set[BuiltIn.OptionRuleOpcode];
            InstructionInstance instance = new(definition);

            SubNode<NodeData> option = new SubNode<NodeData>(instance.ToString(), new(set, definition, instance));

            // Child rules.
            Inspector[] childRules = inspector.GetActiveSubInspectors();
            foreach (Inspector childRule in childRules)
            {
                option.AddChild(Compile(childRule));
            }

            // End of rule.
            option.AddChild(GetEndOfBlock(inspector.InstructionSet));

            return option;
        }
    }
}
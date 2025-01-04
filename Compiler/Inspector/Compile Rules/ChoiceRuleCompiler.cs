using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    public abstract class ChoiceRuleCompiler : RuleCompiler
    {
        /* Public methods. */
        public static SubNode<NodeData> Compile(ChoiceRuleInspector inspector)
        {
            // Main rule.
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = set[BuiltIn.ChoiceRuleOpcode];
            InstructionInstance instance = new(definition);

            SubNode<NodeData> choice = new SubNode<NodeData>(BuiltIn.ChoiceRuleOpcode, new(set, definition, instance));

            // Child rules.
            Inspector[] childRules = inspector.GetActiveSubInspectors();
            foreach (Inspector childRule in childRules)
            {
                choice.AddChild(Compile(childRule));
            }

            // End of rule.
            choice.AddChild(GetEndOfRule(inspector));

            return choice;
        }
    }
}
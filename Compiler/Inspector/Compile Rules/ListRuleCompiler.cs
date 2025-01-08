using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// A compiler that converts a list rule inspector into a graph node.
    /// </summary>
    public abstract class ListRuleCompiler : RuleCompiler
    {
        /* Public methods. */
        public static SubNode<NodeData> Compile(ListRuleInspector inspector)
        {
            // Main rule.
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = set[BuiltIn.ListRuleOpcode];
            InstructionInstance instance = new(definition);

            SubNode<NodeData> list = new SubNode<NodeData>(instance.ToString(), new(set, definition, instance));

            // Child rules.
            Inspector[] childRules = inspector.GetActiveSubInspectors();
            foreach (Inspector childRule in childRules) 
            {
                list.AddChild(Compile(childRule));
            }

            // End of rule.
            list.AddChild(GetEndOfBlock(inspector.InstructionSet));

            return list;
        }
    }
}
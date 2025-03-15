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
        /// <summary>
        /// Compile a list rule inspector into a compiler node hierarchy.
        /// </summary>
        public static SubNode<NodeData> Compile(ListRuleInspector inspector)
        {
            // Main rule.
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = set[BuiltIn.ListRuleOpcode];
            InstructionInstance instance = new(definition);

            SubNode<NodeData> list = CompilerNodeMaker.GetListRule(set);

            // Child rules.
            Inspector[] childRules = inspector.GetActiveSubInspectors();
            foreach (Inspector childRule in childRules) 
            {
                list.AddChild(Compile(childRule));
            }

            // End of rule.
            list.AddChild(CompilerNodeMaker.GetEndOfGroup(inspector.InstructionSet));

            return list;
        }
    }
}
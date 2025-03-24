using Rusty.ISA;
using Rusty.Graphs;

namespace Rusty.ISA.Editor.Compiler
{
    /// <summary>
    /// A compiler that converts a tuple rule inspector into a graph node.
    /// </summary>
    public abstract class TupleRuleCompiler : RuleCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile a tuple rule inspector into a compiler node hierarchy.
        /// </summary>
        public static SubNode<NodeData> Compile(TupleRuleInspector inspector)
        {
            // Main rule.
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = set[BuiltIn.TupleRuleOpcode];
            InstructionInstance instance = new(definition);

            SubNode<NodeData> tuple = CompilerNodeMaker.GetTupleRule(set);

            // Child rules.
            Inspector[] childRules = inspector.GetActiveSubInspectors();
            foreach (Inspector childRule in childRules)
            {
                tuple.AddChild(Compile(childRule));
            }

            // End of rule.
            tuple.AddChild(CompilerNodeMaker.GetEndOfGroup(inspector.InstructionSet));

            return tuple;
        }
    }
}
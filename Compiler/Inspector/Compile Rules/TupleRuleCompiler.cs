using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// A compiler that converts a tuple rule inspector into a graph node.
    /// </summary>
    public abstract class TupleRuleCompiler : RuleCompiler
    {
        /* Public methods. */
        public static SubNode<NodeData> Compile(TupleRuleInspector inspector)
        {
            // Main rule.
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = set[BuiltIn.TupleRuleOpcode];
            InstructionInstance instance = new(definition);

            SubNode<NodeData> tuple = new SubNode<NodeData>(instance.ToString(), new(set, definition, instance));

            // Child rules.
            Inspector[] childRules = inspector.GetActiveSubInspectors();
            foreach (Inspector childRule in childRules)
            {
                tuple.AddChild(Compile(childRule));
            }

            // End of rule.
            tuple.AddChild(GetEndOfBlock(inspector.InstructionSet));

            return tuple;
        }
    }
}
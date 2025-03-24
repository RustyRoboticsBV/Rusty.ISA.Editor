using System;
using Rusty.Graphs;

namespace Rusty.ISA.Editor.Programs.Compiler
{
    /// <summary>
    /// An tuple rule decompiler.
    /// </summary>
    public abstract class TupleRuleDecompiler : RuleDecompiler
    {
        /// <summary>
        /// Apply an tuple rule node to a target inspector.
        /// </summary>
        public static void Apply(Node<NodeData> tupleNode, TupleRuleInspector tupleInspector)
        {
            if (tupleNode.Data.GetOpcode() != BuiltIn.TupleRuleOpcode)
            {
                throw new ArgumentException($"Cannot apply nodes with opcode '{tupleNode.Data.GetOpcode()}' to tuple rule "
                    + "inspectors!");
            }

            // Handle the selected sub-inspector(s).
            Inspector[] subInspectors = tupleInspector.GetActiveSubInspectors();
            for (int i = 0; i < subInspectors.Length && i < tupleNode.Children.Count - 1; i++)
            {
                ApplyRule(tupleNode.Children[i], subInspectors[i]);
            }
        }
    }
}
using System;
using Rusty.Graphs;

namespace Rusty.ISA.ProgramEditor.Compiler
{
    /// <summary>
    /// An list rule decompiler.
    /// </summary>
    public abstract class ListRuleDecompiler : RuleDecompiler
    {
        /// <summary>
        /// Apply an list rule node to a target inspector.
        /// </summary>
        public static void Apply(Node<NodeData> listNode, ListRuleInspector listInspector)
        {
            if (listNode.Data.GetOpcode() != BuiltIn.ListRuleOpcode)
            {
                throw new ArgumentException($"Cannot apply nodes with opcode '{listNode.Data.GetOpcode()}' to list rule "
                    + "inspectors!");
            }

            // Ensure enough list elements.
            listInspector.EnsureElements(listNode.Children.Count - 1);

            // Handle the selected sub-inspector(s).
            Inspector[] subInspectors = listInspector.GetActiveSubInspectors();
            for (int i = 0; i < subInspectors.Length && i < listNode.Children.Count - 1; i++)
            {
                ApplyRule(listNode.Children[i], subInspectors[i]);
            }
        }
    }
}
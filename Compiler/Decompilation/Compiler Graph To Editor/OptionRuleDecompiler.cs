using System;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// An option rule decompiler.
    /// </summary>
    public abstract class OptionRuleDecompiler : RuleDecompiler
    {
        /// <summary>
        /// Apply an option rule node to a target inspector.
        /// </summary>
        public static void Apply(Node<NodeData> optionNode, OptionRuleInspector optionInspector)
        {
            if (optionNode.Data.GetOpcode() != BuiltIn.OptionRuleOpcode)
            {
                throw new ArgumentException($"Cannot apply nodes with opcode '{optionNode.Data.GetOpcode()}' to option rule "
                    + "inspectors!");
            }

            // Tick the checkbox if necessary.
            if (optionNode.Children.Count > 0 && optionNode.Children[0].Data.GetOpcode() != BuiltIn.EndOfGroupOpcode)
                optionInspector.Checked = true;

            // Handle the selected sub-inspector(s).
            Inspector[] subInspectors = optionInspector.GetActiveSubInspectors();
            for (int i = 0; i < subInspectors.Length && i < optionNode.Children.Count - 1; i++)
            {
                ApplyRule(optionNode.Children[i], subInspectors[i]);
            }
        }
    }
}
using System;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// An choice rule decompiler.
    /// </summary>
    public abstract class ChoiceRuleDecompiler : RuleDecompiler
    {
        /// <summary>
        /// Apply an choice rule node to a target inspector.
        /// </summary>
        public static void Apply(Node<NodeData> choiceNode, ChoiceRuleInspector choiceInspector)
        {
            if (choiceNode.Data.GetOpcode() != BuiltIn.ChoiceRuleOpcode)
            {
                throw new ArgumentException($"Cannot apply nodes with opcode '{choiceNode.Data.GetOpcode()}' to choice rule "
                    + "inspectors!");
            }

            // Select the correct choice.
            int selected = -1;
            try
            {
                selected = int.Parse(choiceNode.Data.GetArgument(BuiltIn.ChoiceRuleSelected));
            }
            catch { }

            choiceInspector.Selected = selected;

            // Decompile the node that corresponds to the currently-selected choice.
            Inspector[] subInspectors = choiceInspector.GetActiveSubInspectors();
            for (int i = 0; i < subInspectors.Length && i < choiceNode.Children.Count - 1; i++)
            {
                ApplyRule(choiceNode.Children[i], subInspectors[i]);
            }
        }
    }
}
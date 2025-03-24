using Rusty.Graphs;

namespace Rusty.ISA.ProgramEditor.Compiler
{
    public abstract class SecondaryInstructionsDecompiler
    {
        /// <summary>
        /// Apply a secondary instructions node to a secondary instructions inspector.
        /// </summary>
        public static void Apply(Node<NodeData> secondaryInstructionsNode, SecondaryInstructionsInspector instructionsInspector)
        {
            for (int i = 0; i < secondaryInstructionsNode.Children.Count; i++)
            {
                SubNode<NodeData> child = secondaryInstructionsNode.Children[i];

                // Stop on end-of-group node.
                if (child.Data.GetOpcode() == BuiltIn.EndOfGroupOpcode || i >= instructionsInspector.Inspectors.Count)
                    break;

                // Decompile secondary instruction node.
                Inspector inspector = instructionsInspector.Inspectors[i];

                if (child.Data.GetOpcode() != BuiltIn.EndOfGroupOpcode)
                    RuleDecompiler.ApplyRule(child, inspector);
                else
                    break;
            }
        }
    }
}
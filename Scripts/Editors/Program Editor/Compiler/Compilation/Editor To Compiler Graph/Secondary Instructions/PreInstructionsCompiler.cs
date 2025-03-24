using Rusty.ISA;
using Rusty.Graphs;

namespace Rusty.ISA.ProgramEditor.Compiler
{
    /// <summary>
    /// A compiler that converts pre-instructions inspectors to graph nodes,
    /// </summary>
    public abstract class PreInstructionsCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile a pre-instructions inspector into a compiler node hierarchy.
        /// </summary>
        public static SubNode<NodeData> Compile(PreInstructionsInspector inspector)
        {
            InstructionSet set = inspector.InstructionSet;

            // Create PRE node.
            SubNode<NodeData> node = CompilerNodeMaker.GetPreInstructions(set);

            // Add contents.
            for (int i = 0; i < inspector.Inspectors.Count; i++)
            {
                node.AddChild(RuleCompiler.Compile(inspector.Inspectors[i]));
            }

            // Add end-of-group.
            node.AddChild(CompilerNodeMaker.GetEndOfGroup(set));

            // Return finished node.
            return node;
        }
    }
}
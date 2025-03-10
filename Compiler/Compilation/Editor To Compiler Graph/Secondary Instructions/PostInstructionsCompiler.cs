using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// A compiler that converts post-instructions inspectors to graph nodes,
    /// </summary>
    public abstract class PostInstructionsCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile a post-instructions inspector into a compiler node hierarchy.
        /// </summary>
        public static SubNode<NodeData> Compile(PostInstructionsInspector inspector)
        {
            InstructionSet set = inspector.InstructionSet;

            // Create PRE node.
            SubNode<NodeData> node = CompilerNodeMaker.GetPostInstructions(set);

            // Add contents.
            for (int i = 0; i < inspector.InspectorCount; i++)
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
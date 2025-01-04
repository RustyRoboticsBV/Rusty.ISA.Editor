using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    public abstract class PreInstructionCompiler : InstructionCompiler
    {
        /* Public methods. */
        public static SubNode<NodeData> Compile(PreInstructionInspector inspector)
        {
            return GetInstruction(inspector);
        }
    }
}
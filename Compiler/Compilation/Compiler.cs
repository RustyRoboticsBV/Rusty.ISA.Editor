using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// A base class for all inspector to graph node compilers.
    /// </summary>
    public abstract class Compiler
    {
        /* Protected methods. */
        protected static SubNode<NodeData> GetEndOfBlock(InstructionSet set)
        {
            InstructionDefinition definition = set[BuiltIn.EndOfBlockOpcode];
            InstructionInstance instance = new(definition);

            return new(instance.ToString(), new(set, definition, instance));
        }
    }
}
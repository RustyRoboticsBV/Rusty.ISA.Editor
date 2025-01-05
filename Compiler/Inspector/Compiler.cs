using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// Base class for all inspector to graph node compiler.
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
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    public abstract class GraphEditNodeCompiler : Compiler
    {
        /* Public methods. */
        /// <summary>
        /// Take a graph edit node, and convert it and its inspector data to a compiler node.
        /// Inputs and outputs are NOT added by this method!
        /// </summary>
        public static RootNode<NodeData> Compile(CutsceneGraphNode graphNode)
        {
            return CompilerNodeMaker.CreateHierarchy(
                graphNode.InstructionSet,
                ((int)graphNode.PositionOffset.X).ToString(),
                ((int)graphNode.PositionOffset.Y).ToString(),
                graphNode.NodeInspector.LabelName,
                InstructionCompiler.Compile(graphNode.NodeInspector)
            );
        }
    }
}
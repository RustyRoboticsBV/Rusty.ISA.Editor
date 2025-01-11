using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// An extention of the graph class, tooled towards cutscene compilation/decompilation.
    /// </summary>
    public class CompilerGraph : Graph<NodeData>
    {
        protected override RootNode<NodeData> CreateNode(NodeData data)
        {
            return base.CreateNode(data);
        }
    }
}

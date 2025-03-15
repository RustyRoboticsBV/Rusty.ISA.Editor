using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// An extention of the graph class, tooled towards cutscene compilation/decompilation.
    /// </summary>
    public class CompilerGraph : Graph<NodeData>
    {
        /* Indexers. */
        public new CompilerNode this[int index] => base[index] as CompilerNode;

        /* Protected methods. */
        protected override RootNode<NodeData> CreateNode(NodeData data)
        {
            return base.CreateNode(data);
        }
    }
}

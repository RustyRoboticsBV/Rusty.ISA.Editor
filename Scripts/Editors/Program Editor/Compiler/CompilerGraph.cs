using Rusty.Graphs;

namespace Rusty.ISA.Editor.Compiler
{
    /// <summary>
    /// An extention of the graph class, tooled towards ISA compilation/decompilation.
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

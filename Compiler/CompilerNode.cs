using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// An extention of the node class, tooled towards cutscene compilation/decompilation.
    /// </summary>
    public class CompilerNode : RootNode<NodeData>
    {
        /* Constructors. */
        public CompilerNode() : base() { }

        public CompilerNode(NodeData nodeData) : base(nodeData) { }

        public CompilerNode(SubNode<NodeData> node) : base(node) { }

        /* Casting operators. */
        public static implicit operator CompilerNode(SubNode<NodeData> node)
        {
            return new CompilerNode(node);
        }

        /* Public methods. */
        public override string GetName()
        {
            SubNode<NodeData> startNode = GetStart();
            if (startNode != null)
                return $"{startNode.Data.GetArgument(BuiltIn.StartNameId)}: {base.GetName()}";

            SubNode<NodeData> labelNode = GetLabel();
            if (labelNode != null)
                return $"({labelNode.Data.GetArgument(BuiltIn.LabelNameId)}) {base.GetName()}";

            return base.GetName();
        }

        /// <summary>
        /// Get the start name sub-node. Returns null if it doesn't exist.
        /// </summary>
        public SubNode<NodeData> GetStart()
        {
            return FindSubNode(BuiltIn.StartOpcode);
        }

        /// <summary>
        /// Get the label sub-node. Returns null if it doesn't exist.
        /// </summary>
        public SubNode<NodeData> GetLabel()
        {
            return FindSubNode(BuiltIn.LabelOpcode);
        }

        public bool IsEnd()
        {
            try
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    for (int j = 0; j < Children[i].Children.Count; j++)
                    {
                        if (Children[i].Children[j].Data.GetOpcode() == BuiltIn.EndOpcode)
                            return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool IsGoto()
        {
            try
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    for (int j = 0; j < Children[i].Children.Count; j++)
                    {
                        if (Children[i].Children[j].Data.GetOpcode() == BuiltIn.GotoOpcode)
                            return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private SubNode<NodeData> FindSubNode(string opcode)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].Data.GetOpcode() == opcode)
                    return Children[i];
            }
            return null;
        }
    }
}

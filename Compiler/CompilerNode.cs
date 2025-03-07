using Rusty.Graphs;
using Rusty.Cutscenes;

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
        /// Get the output data of a node.
        /// </summary>
        public OutputData GetOutputData()
        {
            return GetOutputData(this, null);
        }

        /// <summary>
        /// Get the start name sub-node. Returns null if it doesn't exist.
        /// </summary>
        public SubNode<NodeData> GetStart()
        {
            return FindSubNode(BuiltIn.BeginOpcode);
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

        /// <summary>
        /// Get the instruction's sub-node.
        /// </summary>
        public SubNode<NodeData> GetInstruction()
        {
            try
            {
                return Children[^2].Children[^2];
            }
            catch
            {
                return null;
            }
        }

        /* Private methods. */
        private SubNode<NodeData> FindSubNode(string opcode)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].Data.GetOpcode() == opcode)
                    return Children[i];
            }
            return null;
        }

        /// <summary>
        /// Helper method for GetOutputData. Recursively checks a node and all child nodes.
        /// </summary>
        private OutputData GetOutputData(Node<NodeData> node, OutputData result)
        {
            // Create new output data object.
            if (result == null)
            {
                result = new OutputData();
                result.HasDefaultOutput = true;
            }

            // If end: we don't have a default output.
            if (IsEnd())
                result.HasDefaultOutput = false;

            // Else, handle output arguments.
            else
            {
                for (int i = 0; i < node.Data.Definition.Parameters.Length; i++)
                {
                    if (node.Data.Definition.RemovesDefaultOutput())
                        result.HasDefaultOutput = false;
                    if (node.Data.Definition.Parameters[i] is OutputParameter output)
                    {
                        result.AddOutput(node, i);
                    }
                }

                // Handle child nodes.
                foreach (SubNode<NodeData> sub in node.Children)
                {
                    GetOutputData(sub, result);
                }
            }

            return result;
        }
    }
}

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
            if (node != null)
            {
                CompilerNode result = new();
                result.Data = node.Data;
                for (int i = 0; i < node.Children.Count; i++)
                {
                    result.AddChild(node.Children[i]);
                }
                return result;
            }
            else
                return null;
        }

        /* Public methods. */
        public override string GetName()
        {
            SubNode<NodeData> beginNode = GetBegin();
            if (beginNode != null)
                return $"{beginNode.Data.GetArgument(BuiltIn.BeginNameID)}: {base.GetName()}";

            SubNode<NodeData> labelNode = GetLabel();
            if (labelNode != null)
                return $"({labelNode.Data.GetArgument(BuiltIn.LabelNameID)}) {base.GetName()}";

            return base.GetName();
        }

        /// <summary>
        /// Check if the node represents an END instruction.
        /// </summary>
        public bool IsEnd()
        {
            return FindSubNode(BuiltIn.EndOpcode) != null;
        }

        /// <summary>
        /// Check if the node represents a GTO instruction.
        /// </summary>
        public bool IsGoto()
        {
            return FindSubNode(BuiltIn.GotoOpcode) != null;
        }

        /// <summary>
        /// Get the label sub-node. Returns null if it doesn't exist.
        /// </summary>
        public SubNode<NodeData> GetLabel()
        {
            return FindSubNode(BuiltIn.LabelOpcode);
        }

        /// <summary>
        /// Get the start name sub-node. Returns null if it doesn't exist.
        /// </summary>
        public SubNode<NodeData> GetBegin()
        {
            return FindSubNode(BuiltIn.BeginOpcode);
        }

        /// <summary>
        /// Get the inspector sub-node. Returns null if it doesn't exist.
        /// </summary>
        public SubNode<NodeData> GetInspector()
        {
            return FindSubNode(BuiltIn.InspectorOpcode);
        }

        /// <summary>
        /// Get the main instruction sub-node. Returns null if it doesn't exist.
        /// </summary>
        public SubNode<NodeData> GetMainInstruction()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                switch (Children[i].Data.GetOpcode())
                {
                    case BuiltIn.EndOpcode:
                    case BuiltIn.GotoOpcode:
                        return Children[i];
                    case BuiltIn.InspectorOpcode:
                        for (int j = 0; j < Children[i].Children.Count; j++)
                        {
                            switch (Children[i].Children[j].Data.GetOpcode())
                            {
                                case BuiltIn.PreInstructionOpcode:
                                case BuiltIn.PostInstructionOpcode:
                                    break;
                                default:
                                    return Children[i].Children[j];
                            }
                        }
                        break;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the pre-instructions sub-node. Returns null if it doesn't exist.
        /// </summary>
        public SubNode<NodeData> GetPreInstructions()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                switch (Children[i].Data.GetOpcode())
                {
                    case BuiltIn.InspectorOpcode:
                        for (int j = 0; j < Children[i].Children.Count; j++)
                        {
                            switch (Children[i].Children[j].Data.GetOpcode())
                            {
                                case BuiltIn.PreInstructionOpcode:
                                    return Children[i].Children[j];
                                default:
                                    break;
                            }
                        }
                        break;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the post-instructions sub-node. Returns null if it doesn't exist.
        /// </summary>
        public SubNode<NodeData> GetPostInstructions()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                switch (Children[i].Data.GetOpcode())
                {
                    case BuiltIn.InspectorOpcode:
                        for (int j = 0; j < Children[i].Children.Count; j++)
                        {
                            switch (Children[i].Children[j].Data.GetOpcode())
                            {
                                case BuiltIn.PostInstructionOpcode:
                                    return Children[i].Children[j];
                                default:
                                    break;
                            }
                        }
                        break;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the output data of a node.
        /// </summary>
        public OutputData GetOutputData()
        {
            return GetOutputData(this, null);
        }

        /* Private methods. */
        /// <summary>
        /// Find a subnode with some opcode.
        /// </summary>
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
                        result.AddOutput(node, i);
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
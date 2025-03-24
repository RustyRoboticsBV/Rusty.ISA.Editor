using System.Collections.Generic;
using Rusty.Graphs;

namespace Rusty.ISA.ProgramEditor.Compiler
{
    /// <summary>
    /// A list of output information of a node.
    /// </summary>
    public class OutputData
    {
        /* Public types. */
        /// <summary>
        /// The data of a single output.
        /// </summary>
        public class Output
        {
            /// <summary>
            /// The node containing the parameter.
            /// </summary>
            public Node<NodeData> Node { get; set; }
            /// <summary>
            /// The argument's index.
            /// </summary>
            public int ArgumentIndex { get; set; }
            /// <summary>
            /// The argument's value.
            /// </summary>
            public string ArgumentValue => Node.Data.GetArgument(ArgumentIndex);

            public Output(Node<NodeData> node, int argumentIndex)
            {
                Node = node;
                ArgumentIndex = argumentIndex;
            }
        }

        /* Public properties. */
        public Node<NodeData> Source { get; set; }
        public int ArgumentCount => ArgumentOutputs.Count;
        public bool HasDefaultOutput { get; set; } = true;

        /* Private properties. */
        private List<Output> ArgumentOutputs { get; } = new List<Output>();

        /* Public methods. */
        public void AddOutput(Node<NodeData> node, int argumentIndex)
        {
            ArgumentOutputs.Add(new Output(node, argumentIndex));
        }

        public Output GetOutput(int argumentIndex)
        {
            return ArgumentOutputs[argumentIndex];
        }
    }
}
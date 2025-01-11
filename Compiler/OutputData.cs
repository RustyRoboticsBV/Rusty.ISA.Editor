using Rusty.Graphs;
using System.Collections.Generic;

namespace Rusty.CutsceneEditor.Compiler
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
            public Node<NodeData> Source { get; set; }
            public int ArgumentIndex { get; set; }

            public Output(Node<NodeData> source, int argumentIndex)
            {
                Source = source;
                ArgumentIndex = argumentIndex;
            }
        }

        /* Public properties. */
        public Node<NodeData> Source { get; set; }
        public List<Output> ArgumentOutputs { get; } = new();
        public bool HasDefaultOutput { get; set; }
    }
}
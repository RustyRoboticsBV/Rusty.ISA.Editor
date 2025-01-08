using System.Collections.Generic;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// A list of output information of a node.
    /// </summary>
    public class OutputData<T>
    {
        /* Public types. */
        /// <summary>
        /// The data of a single output.
        /// </summary>
        public class Output
        {
            public T Source { get; set; }
            public int ArgumentIndex { get; set; }

            public Output(T source, int argumentIndex)
            {
                Source = source;
                ArgumentIndex = argumentIndex;
            }
        }

        /* Public properties. */
        public T Source { get; set; }
        public List<Output> ArgumentOutputs { get; } = new();
        public bool HasDefaultOutput { get; set; }
    }
}
using Godot;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// A compiler that compiles a compiler node into code.
    /// </summary>
    public class NodeCompiler : Compiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile a node and its child nodes.
        /// </summary>
        public static string Compile(Node<NodeData> node)
        {
            string code = GetCode(node);

            for (int i = 0; i < node.Children.Count; i++)
            {
                code += "\n" + Compile(node.Children[i]);
            }

            return code;
        }

        /* Private methods. */
        /// <summary>
        /// Get the code of a node, but NOT its child nodes.
        /// </summary>
        private static string GetCode(Node<NodeData> node)
        {
            string code = node.Data.Instance.Opcode;
            for (int i = 0; i < node.Data.Instance.Arguments.Length; i++)
            {
                string arg = node.Data.Instance.Arguments[i];

                if (arg == null)
                {
                    GD.PrintErr("Argument " + node.Data.Definition.Parameters[i] + " of " + node.Data.Definition.Opcode + " is null.");
                    continue;
                }

                // Duplicate double quotes.
                arg = arg.Replace("\"", "\"\"");

                // Add quotes if the arg contains a comma or double-quote.
                if (arg.Contains(',') || arg.Contains("\""))
                    arg = $"\"{arg}\"";

                // Add to code.
                code += "," + arg;
            }
            return code;
        }
    }
}
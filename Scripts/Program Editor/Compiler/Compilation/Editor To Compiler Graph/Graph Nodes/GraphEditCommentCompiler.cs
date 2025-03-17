using Godot;
using Rusty.ISA;

namespace Rusty.ISA.Editor.Compiler
{
    public abstract class GraphEditCommentCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Take a graph edit comment node, and convert it and its inspector data to a compiler node.
        /// </summary>
        public static CompilerNode Compile(GraphComment graphNode)
        {
            InstructionSet set = graphNode.InstructionSet;
            int x = Mathf.RoundToInt(graphNode.PositionOffset.X);
            int y = Mathf.RoundToInt(graphNode.PositionOffset.Y);
            string text = graphNode.Inspector.CommentText;
            text = text.Replace("\\n", "\\\\n").Replace("\n", "\\n");

            // Create CMT node.
            CompilerNode comment = CompilerNodeMaker.GetComment(set, x.ToString(), y.ToString(), text);

            // Add EOG sub-node.
            comment.AddChild(CompilerNodeMaker.GetEndOfGroup(set));

            // Return finished node.
            return comment;
        }
    }
}
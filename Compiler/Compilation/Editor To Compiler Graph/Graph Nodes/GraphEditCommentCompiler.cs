using Godot;
using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    public abstract class GraphEditCommentCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Take a graph edit comment node, and convert it and its inspector data to a compiler node.
        /// </summary>
        public static CompilerNode Compile(CutsceneGraphNode graphNode)
        {
            InstructionSet set = graphNode.InstructionSet;
            int x = Mathf.RoundToInt(graphNode.PositionOffset.X);
            int y = Mathf.RoundToInt(graphNode.PositionOffset.Y);
            string text = graphNode.NodeInspector.GetParameterInspector(BuiltIn.CommentText).ValueObj as string;

            // Create CMT node.
            CompilerNode comment = CompilerNodeMaker.GetComment(set, x.ToString(), y.ToString(), text);

            // Add EOG sub-node.
            comment.AddChild(CompilerNodeMaker.GetEndOfGroup(set));

            // Return finished node.
            return comment;
        }
    }
}
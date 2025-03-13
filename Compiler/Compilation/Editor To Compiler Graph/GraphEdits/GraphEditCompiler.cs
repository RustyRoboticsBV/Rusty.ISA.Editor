using System.Xml.Linq;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// Compile a graph edit into a code string.
    /// </summary>
    public static class GraphEditCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile a graph edit into a compiler graph that represents the cutscene program.
        public static CompilerGraph Compile(CutsceneGraphEdit graphEdit)
        {
            // 1. Create empty graph.
            CompilerGraph graph = new();
            graph.Data.Set = graphEdit.InstructionSet;

            // 2. Create nodes.
            for (int i = 0; i < graphEdit.Nodes.Count; i++)
            {
                CompilerNode node = GraphEditNodeCompiler.Compile(graphEdit.Nodes[i]);
                graph.AddNode(node);
            }

            // 3. Connect nodes.
            for (int i = 0; i < graphEdit.Nodes.Count; i++)
            {
                // Get editor & compiler node.
                CutsceneGraphInstruction fromEditorNode = graphEdit.Nodes[i];
                CompilerNode fromCompilerNode = graph[i];

                // For each output slot...
                for (int j = 0; j < fromEditorNode.Slots.Count; j++)
                {
                    // Add empty outputs for non-connected slots.
                    if (fromEditorNode.Slots[j].Output == null)
                        fromCompilerNode.ConnectTo(null);

                    // Else, connect nodes.
                    else
                    {
                        CutsceneGraphInstruction toEditorNode = fromEditorNode.Slots[j].Output.Node;
                        int toNodeIndex = graphEdit.Nodes.IndexOf(toEditorNode);
                        CompilerNode toCompilerNode = graph[toNodeIndex];

                        fromCompilerNode.ConnectTo(toCompilerNode);
                    }
                }
            }

            // 4. Insert frames.
            GraphEditFrameCompiler.ResetIDGenerator();
            for (int i = 0; i < graphEdit.Frames.Count; i++)
            {
                CompilerNode frame = GraphEditFrameCompiler.Compile(graphEdit.Frames[i]);
                graph.InsertNode(i, frame);
            }

            // 5. Insert comments.
            for (int i = 0; i < graphEdit.Comments.Count; i++)
            {
                CompilerNode omment = GraphEditCommentCompiler.Compile(graphEdit.Comments[i]);
                graph.InsertNode(i, omment);
            }

            return graph;
        }
    }
}
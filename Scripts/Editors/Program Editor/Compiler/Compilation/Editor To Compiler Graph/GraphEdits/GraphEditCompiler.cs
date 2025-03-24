using System.Collections.Generic;
using Godot;
using Rusty.ISA;
using Rusty.Graphs;

namespace Rusty.ISA.Editor.Programs.Compiler
{
    /// <summary>
    /// Compile a graph edit into a code string.
    /// </summary>
    public static class GraphEditCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile a graph edit into a compiler graph that represents the program.
        public static CompilerGraph Compile(ProgramGraphEdit graphEdit)
        {
            InstructionSet set = graphEdit.InstructionSet;

            // 1. Create empty graph.
            CompilerGraph graph = new();
            graph.Data.Set = set;

            // 2. Create nodes.
            for (int i = 0; i < graphEdit.Nodes.Count; i++)
            {
                // Compile node.
                CompilerNode node = GraphEditNodeCompiler.Compile(graphEdit.Nodes[i]);
                graph.AddNode(node);

                // Add frame member.
                AddFrameMember(node, graphEdit.Nodes[i], graphEdit);
            }

            // 3. Connect nodes.
            for (int i = 0; i < graphEdit.Nodes.Count; i++)
            {
                // Get editor & compiler node.
                GraphInstruction fromEditorNode = graphEdit.Nodes[i];
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
                        GraphInstruction toEditorNode = fromEditorNode.Slots[j].Output.Node;
                        int toNodeIndex = graphEdit.Nodes.IndexOf(toEditorNode);
                        CompilerNode toCompilerNode = graph[toNodeIndex];

                        fromCompilerNode.ConnectTo(toCompilerNode);
                    }
                }
            }

            // 4. Insert comments.
            for (int i = 0; i < graphEdit.Comments.Count; i++)
            {
                // Compile comment.
                CompilerNode comment = GraphEditCommentCompiler.Compile(graphEdit.Comments[i]);
                graph.InsertNode(i, comment);

                // Add frame member.
                AddFrameMember(comment, graphEdit.Comments[i], graphEdit);
            }

            // 5. Insert frames.
            GraphEditFrameCompiler.ResetIDGenerator();
            for (int i = 0; i < graphEdit.Frames.Count; i++)
            {
                // Compile frame.
                CompilerNode frame = GraphEditFrameCompiler.Compile(graphEdit.Frames[i]);
                graph.InsertNode(i, frame);

                // Add frame member.
                AddFrameMember(frame, graphEdit.Frames[i], graphEdit);
            }

            return graph;
        }

        /* Private methods. */
        private static void AddFrameMember(CompilerNode node, IGraphElement element, ProgramGraphEdit graphEdit)
        {
            if (element.Frame != null)
            {
                int frameID = graphEdit.Frames.IndexOf(element.Frame);
                SubNode<NodeData> member = CompilerNodeMaker.GetFrameMember(graphEdit.InstructionSet, frameID.ToString());
                node.InsertChild(0, member);
            }
        }
    }
}
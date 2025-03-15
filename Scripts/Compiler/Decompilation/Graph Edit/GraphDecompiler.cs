using Godot;
using System.Collections.Generic;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// A class that can take a compiler graph and spawn cutscene graph edit nodes.
    /// </summary>
    public abstract class GraphDecompiler
    {
        /* Public methods. */
        public static void Spawn(CutsceneGraphEdit graphEdit, CompilerGraph graph)
        {
            Dictionary<CompilerNode, IGraphElement> elementMap = new();
            Dictionary<string, CutsceneGraphFrame> frameMap = new();
            CompilerNode metadata = null;

            // Spawn nodes.
            for (int i = 0; i < graph.Count; i++)
            {
                // Get compiler node.
                CompilerNode node = graph[i];

                // Determine graph element type...
                switch (node.Data.GetOpcode())
                {
                    case BuiltIn.MetadataOpcode:
                        if (metadata == null)
                            metadata = node;
                        else
                        {
                            GD.PrintErr($"Encountered second metadata instruction at line #{i}. Only one metadata instruction is "
                                + "allowed per cutscene program!");
                        }
                        break;
                    case BuiltIn.NodeOpcode:
                        if (!node.IsEnd() && !node.IsGoto())
                            SpawnInstruction(graphEdit, node, i, elementMap);
                        break;
                    case BuiltIn.CommentOpcode:
                        SpawnComment(graphEdit, node, elementMap);
                        break;
                    case BuiltIn.FrameOpcode:
                        SpawnFrame(graphEdit, node, frameMap, elementMap);
                        break;
                    default:
                        GD.PrintErr($"Encountered illegal top-layer instruction with opcode '{node.Data.GetOpcode()}' at line "
                            + $"#{i}. Only '{BuiltIn.MetadataOpcode}', '{BuiltIn.NodeOpcode}', '{BuiltIn.CommentOpcode}' and "
                            + $"'{BuiltIn.FrameOpcode}' are allowed.");
                        break;
                }
            }

            // Connect nodes & add them to frames.
            for (int i = 0; i < graph.Count; i++)
            {
                // Get compiler & graph edit nodes.
                CompilerNode node = graph[i];

                // If the compiler node wasn't a key on the node map, then it was either a metadata node or an erroneous node.
                // Skip it.
                if (!elementMap.ContainsKey(node))
                    continue;

                // Retrieve element.
                IGraphElement element = elementMap[node];

                // Add to frame (if necessary).
                SubNode<NodeData> frameMember = node.GetFrameMember();
                if (frameMember != null)
                {
                    string frameID = frameMember.Data.GetArgument(BuiltIn.FrameMemberID);
                    if (frameMap.ContainsKey(frameID))
                        frameMap[frameID].AddElement(element);
                }

                // If we're an instruction node...
                if (element is CutsceneGraphInstruction editorNode)
                {
                    // Ensure enough slots on the editor node.
                    editorNode.EnsureSlots(node.Outputs.Count);

                    // Connect outputs.
                    for (int j = 0; j < node.Outputs.Count; j++)
                    {
                        CompilerNode toNode = node.Outputs[j].ToNode as CompilerNode;
                        if (toNode.IsGoto())
                            toNode = toNode.Outputs[0].ToNode as CompilerNode;
                        else if (toNode.IsEnd())
                            continue;

                        graphEdit.ConnectNode(editorNode, j, elementMap[toNode] as CutsceneGraphInstruction);
                    }
                }
            }
        }


        /* Private methods. */
        private static void SpawnInstruction(CutsceneGraphEdit graphEdit, CompilerNode node, int lineNumber,
            Dictionary<CompilerNode, IGraphElement> elementMap)
        {
            // Get instruction.
            SubNode<NodeData> mainInstruction = node.GetMainInstruction();

            // Do nothing if the instruction instruction has no node definition.
            if (mainInstruction.Data.Definition.EditorNode == null)
            {
                GD.PrintErr($"Encountered node with main instruction '{mainInstruction.Data.GetOpcode()}' at line #{lineNumber}, "
                    + "but instructions of this type have no editor node information! The node could not be spawned.");
                return;
            }

            // Get node position.
            float x = 0;
            try
            {
                x = float.Parse(node.Data.GetArgument(BuiltIn.NodeX));
            }
            catch { }

            float y = 0;
            try
            {
                y = float.Parse(node.Data.GetArgument(BuiltIn.NodeY));
            }
            catch { }

            // Spawn node.
            CutsceneGraphInstruction editorNode = graphEdit.SpawnNode(mainInstruction.Data.Definition, new Vector2(x, y));

            // Fill inspector by decompiling sub-nodes.
            NodeInstructionInspector inspector = editorNode.Inspector;
            InspectorDecompiler.Apply(node.GetInspector(), inspector);

            // Add start point name.
            SubNode<NodeData> beginName = node.GetBegin();
            if (beginName != null)
                inspector.LabelName = beginName.Data.GetArgument(BuiltIn.BeginName);

            // Force-update node's appearance.
            editorNode.ForceUpdate();

            // Add to element map.
            elementMap.Add(node, editorNode);
        }

        private static void SpawnComment(CutsceneGraphEdit graphEdit, CompilerNode node,
            Dictionary<CompilerNode, IGraphElement> elementMap)
        {
            // Get node position.
            float x = 0;
            try
            {
                x = float.Parse(node.Data.GetArgument(BuiltIn.CommentX));
            }
            catch { }

            float y = 0;
            try
            {
                y = float.Parse(node.Data.GetArgument(BuiltIn.CommentY));
            }
            catch { }

            // Spawn node.
            CutsceneGraphComment editorComment = graphEdit.SpawnComment(new Vector2(x, y));
            editorComment.Inspector.CommentText = node.Data.GetArgument(BuiltIn.CommentText);

            // Add to element map.
            elementMap.Add(node, editorComment);
        }

        private static void SpawnFrame(CutsceneGraphEdit graphEdit, CompilerNode node,
            Dictionary<string, CutsceneGraphFrame> frameMap, Dictionary<CompilerNode, IGraphElement> elementMap)
        {
            // Get node position.
            float x = 0;
            try
            {
                x = float.Parse(node.Data.GetArgument(BuiltIn.FrameX));
            }
            catch { }

            float y = 0;
            try
            {
                y = float.Parse(node.Data.GetArgument(BuiltIn.FrameY));
            }
            catch { }

            float width = 0;
            try
            {
                width = float.Parse(node.Data.GetArgument(BuiltIn.FrameWidth));
            }
            catch { }

            float height = 0;
            try
            {
                height = float.Parse(node.Data.GetArgument(BuiltIn.FrameHeight));
            }
            catch { }

            // Spawn node.
            CutsceneGraphFrame editorFrame = graphEdit.SpawnFrame(new Vector2(x, y));
            editorFrame.Size = new Vector2(width, height);
            editorFrame.Inspector.TitleText = node.Data.GetArgument(BuiltIn.FrameTitle);

            try
            {
                editorFrame.Inspector.Color = Color.FromHtml(node.Data.GetArgument(BuiltIn.FrameColor));
            }
            catch { }

            // Add to frame map and element map.
            frameMap.Add(node.Data.GetArgument(BuiltIn.FrameID), editorFrame);
            elementMap.Add(node, editorFrame);
        }
    }
}
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
            Dictionary<CompilerNode, CutsceneGraphNode> nodeMap = new();

            // Spawn nodes.
            for (int i = 0; i < graph.Count; i++)
            {
                // Get compiler node.
                CompilerNode node = graph[i];

                // TODO: actually do something with the metadata.
                if (node.Data.GetOpcode() == BuiltIn.MetadataOpcode)
                    continue;

                // Get instruction.
                SubNode<NodeData> mainInstruction = node.GetMainInstruction();

                // Do nothing if the instruction instruction has no node definition.
                if (mainInstruction.Data.Definition.EditorNode == null)
                    continue;

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
                CutsceneGraphNode editorNode = graphEdit.Spawn(mainInstruction.Data.Definition, new Vector2(x, y));
                nodeMap.Add(node, editorNode);

                // Fill inspector.
                NodeInstructionInspector inspector = editorNode.NodeInspector;

                SubNode<NodeData> startName = node.GetBegin();
                if (startName != null)
                    inspector.LabelName = startName.Data.GetArgument(BuiltIn.BeginName);

                InspectorDecompiler.Apply(node.GetInspector(), inspector);

                // Force-update node's appearance.
                editorNode.ForceUpdate();
            }

            // Connect nodes.
            for (int i = 0; i < graph.Count; i++)
            {
                // Get compiler & graph edit nodes.
                CompilerNode node = graph[i];

                if (node.Data.GetOpcode() == BuiltIn.MetadataOpcode)
                    continue;

                if (!nodeMap.ContainsKey(node))
                    continue;

                CutsceneGraphNode editorNode = nodeMap[node];

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

                    graphEdit.ConnectNode(editorNode, j, nodeMap[toNode]);
                }
            }
        }
    }
}
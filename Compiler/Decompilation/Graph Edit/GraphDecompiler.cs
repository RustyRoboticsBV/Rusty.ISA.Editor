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

                // Get instruction.
                SubNode<NodeData> mainInstruction = node.GetMainInstruction();

                // Do nothing if the instruction instruction has no node definition.
                if (mainInstruction.Data.Definition.EditorNode == null)
                    continue;

                // Get node position.
                float x = 0;
                try
                {
                    x = float.Parse(node.Data.GetArgument(BuiltIn.NodeXID));
                }
                catch { }

                float y = 0;
                try
                {
                    y = float.Parse(node.Data.GetArgument(BuiltIn.NodeYID));
                }
                catch { }

                // Spawn node.
                CutsceneGraphNode editorNode = graphEdit.Spawn(mainInstruction.Data.Definition, new Vector2(x, y));
                nodeMap.Add(node, editorNode);

                // Fill inspector.
                NodeInstructionInspector inspector = editorNode.NodeInspector;

                SubNode<NodeData> startName = node.GetBegin();
                if (startName != null)
                    inspector.LabelName = startName.Data.GetArgument(BuiltIn.BeginNameID);

                HandleInspector(inspector, node.GetInspector());

                // Force-update node's appearance.
                editorNode.ForceUpdate();
            }

            // Connect nodes.
            for (int i = 0; i < graph.Count; i++)
            {
                // Get compiler & graph edit nodes.
                CompilerNode node = graph[i];

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

        /* Private methods. */
        private static void HandleInspector(InstructionInspector inspector, Node<NodeData> node)
        {
            SubNode<NodeData> preInstructions = null;
            SubNode<NodeData> postInstructions = null;
            SubNode<NodeData> mainInstruction = null;
            for (int i = 0; i < node.Children.Count; i++)
            {
                switch (node.Children[i].Data.GetOpcode())
                {
                    case BuiltIn.PreInstructionOpcode:
                        if (preInstructions == null)
                            preInstructions = node.Children[i];
                        break;
                    case BuiltIn.PostInstructionOpcode:
                        if (postInstructions == null)
                            postInstructions = node.Children[i];
                        break;
                    case BuiltIn.EndOfGroupOpcode:
                        break;
                    default:
                        if (mainInstruction == null)
                            mainInstruction = node.Children[i];
                        break;
                }
            }

            if (mainInstruction != null)
                HandleInstruction(inspector, mainInstruction);
            if (preInstructions != null)
                HandlePreInstructions(inspector.PreInstructions, preInstructions);
            if (postInstructions != null)
                HandlePostInstructions(inspector.PostInstructions, postInstructions);
        }

        private static void HandleInstruction(InstructionInspector inspector, Node<NodeData> node)
        {
            inspector.SetArguments(node.Data.Instance);
        }

        private static void HandlePreInstructions(PreInstructionsInspector inspector, Node<NodeData> node)
        {
            for (int i = 0; i < node.Children.Count; i++)
            {
                if (node.Children[i].Data.GetOpcode() != BuiltIn.EndOfGroupOpcode)
                    HandleRule(inspector.Inspectors[i], node.Children[i]);
            }
        }

        private static void HandlePostInstructions(PostInstructionsInspector inspector, Node<NodeData> node)
        {
            for (int i = 0; i < node.Children.Count; i++)
            {
                if (node.Children[i].Data.GetOpcode() != BuiltIn.EndOfGroupOpcode)
                    HandleRule(inspector.Inspectors[i], node.Children[i]);
            }
        }

        private static void HandleRule(Inspector inspector, Node<NodeData> node)
        {
            switch (inspector)
            {
                case InstructionInspector instruction:
                    HandleInspector(instruction, node);
                    break;
                case OptionRuleInspector optionRule:
                    HandleOptionRule(optionRule, node);
                    break;
                case ChoiceRuleInspector choiceRule:
                    HandleChoiceRule(choiceRule, node);
                    break;
                case TupleRuleInspector tupleRule:
                    HandleTupleRule(tupleRule, node);
                    break;
                case ListRuleInspector listRule:
                    HandleListRule(listRule, node);
                    break;
            }
        }

        private static void HandleOptionRule(OptionRuleInspector inspector, Node<NodeData> node)
        {
            // Enable the checkbox if necessary.
            if (node.Children.Count > 1)
                inspector.Checked = true;

            // Handle the selected sub-inspector(s).
            Inspector[] subInspectors = inspector.GetActiveSubInspectors();
            for (int i = 0; i < subInspectors.Length && i < node.Children.Count - 1; i++)
            {
                HandleRule(subInspectors[i], node.Children[i]);
            }
        }

        private static void HandleChoiceRule(ChoiceRuleInspector inspector, Node<NodeData> node)
        {
            // Select the correct choice.
            int selected = -1;
            try
            {
                selected = int.Parse(node.Data.GetArgument(BuiltIn.ChoiceRuleSelectedID));
            }
            catch { }

            inspector.Selected = selected;

            // Handle the selected sub-inspector(s).
            Inspector[] subInspectors = inspector.GetActiveSubInspectors();
            for (int i = 0; i < subInspectors.Length && i < node.Children.Count - 1; i++)
            {
                HandleRule(subInspectors[i], node.Children[i]);
            }
        }

        private static void HandleTupleRule(TupleRuleInspector inspector, Node<NodeData> node)
        {
            // Handle the selected sub-inspector(s).
            Inspector[] subInspectors = inspector.GetActiveSubInspectors();
            for (int i = 0; i < subInspectors.Length && i < node.Children.Count - 1; i++)
            {
                HandleRule(subInspectors[i], node.Children[i]);
            }
        }

        private static void HandleListRule(ListRuleInspector inspector, Node<NodeData> node)
        {
            // Ensure enough list elements.
            inspector.EnsureElements(node.Children.Count - 1);

            // Handle the selected sub-inspector(s).
            Inspector[] subInspectors = inspector.GetActiveSubInspectors();
            for (int i = 0; i < subInspectors.Length && i < node.Children.Count - 1; i++)
            {
                HandleRule(subInspectors[i], node.Children[i]);
            }
        }
    }
}
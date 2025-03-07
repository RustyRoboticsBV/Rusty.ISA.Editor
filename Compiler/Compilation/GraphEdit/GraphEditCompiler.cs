using Godot;
using System;
using Rusty.Graphs;
using Rusty.Maps;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// Compile a graph edit into a code string.
    /// </summary>
    public abstract class GraphEditCompiler : Compiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile a graph edit into a compiler graph that represents the finished program. It consists of the following steps:]
        /// <br/>1. Create non-connected compiler nodes, one for each graph edit node.
        /// <br/>2. Connect all the compiler nodes, according to connections in the graph edit.
        /// <br/>3. Find the start nodes.
        /// <br/>4. Figure out the execution order.
        /// <br/>   4.1 Insert end nodes while doing this.
        /// <br/>   4.2 Insert go-to's while doing this.
        /// <br/>   4.3 Set node labels while doing this.
        /// <br/>   4.4 Set output parameter values while doing this.
        /// <br/>5. Compile to code.
        /// </summary>
        public static string Compile(CutsceneGraphEdit graphEdit)
        {
            // 1. Create compiler node graph.
            CompilerGraph graph = new();
            graph.Data.Set = graphEdit.InstructionSet;
            for (int i = 0; i < graphEdit.Nodes.Count; i++)
            {
                var node = GraphEditNodeCompiler.Compile(graphEdit.Nodes[i]);
                graph.AddNode(node);
            }

            // 2. Connect nodes.
            for (int i = 0; i < graphEdit.Nodes.Count; i++)
            {
                // Get editor & compiler node.
                CutsceneGraphNode fromEditorNode = graphEdit.Nodes[i];
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
                        CutsceneGraphNode toEditorNode = fromEditorNode.Slots[j].Output.Node;
                        int toNodeIndex = graphEdit.Nodes.IndexOf(toEditorNode);
                        CompilerNode toCompilerNode = graph[toNodeIndex];

                        fromCompilerNode.ConnectTo(toCompilerNode);
                    }
                }
            }

            // 3. Figure out start nodes.
            int[] startNodes = graph.FindStartNodes();

            // 4. Figure out execution order, set labels, insert go-to's and set output arguments.
            BiDict<int, CompilerNode> executionOrder = new();
            int nextLabel = 0;
            for (int i = 0; i < startNodes.Length; i++)
            {
                CompilerNode startNode = graph[startNodes[i]];
                if (executionOrder.ContainsRight(startNode))
                    continue;

                ProcessSubGraph(startNode, graph, executionOrder, ref nextLabel);
            }

            // Print finished graph.
            GD.Print("Compiled graph:\n" + graph);

            // 5. Compile to code.
            string code = "";
            for (int i = 0; i < executionOrder.Count; i++)
            {
                if (code != "")
                    code += "\n";
                code += NodeCompiler.Compile(executionOrder[i]);
            }
            return code;
        }

        /* Private methods. */
        /// <summary>
        /// Helper function for step 4 of the compilation process.
        /// It figures out the execution order of the graph, sets labels, inserts go-to's and sets output arguments.
        /// </summary>
        private static void ProcessSubGraph(CompilerNode node, CompilerGraph graph,
            BiDict<int, CompilerNode> executionOrder, ref int nextLabel)
        {
            // Add to execution order.
            executionOrder.Add(executionOrder.Count, node);

            // Continue with output nodes.
            for (int i = 0; i < node.Outputs.Count; i++)
            {
                CompilerNode toNode = node.Outputs[i].ToNode as CompilerNode;

                // If there was no successor node, add an end instead.
                if (toNode == null)
                {
                    // Create end node.
                    CompilerNode end = CompilerNodeMaker.CreateHierarchy(graph.Data.Set, BuiltIn.EndOpcode);
                    graph.AddNode(end);

                    // Connect it.
                    node.Outputs[i].ConnectTo(end);

                    // Add to execution order.
                    executionOrder.Add(executionOrder.Count, end);
                }

                // If the target node has been added already, add a go-to instead.
                else if (executionOrder.ContainsRight(toNode))
                {
                    // Create goto node.
                    CompilerNode gto = CompilerNodeMaker.CreateHierarchy(graph.Data.Set, BuiltIn.GotoOpcode);
                    graph.AddNode(gto);

                    // Connect it.
                    node.Outputs[i].ConnectTo(gto);
                    gto.ConnectTo(toNode);

                    // Add to execution order.
                    executionOrder.Add(executionOrder.Count, gto);

                    // Set output parameter.
                    SetOutputArguments(gto, ref nextLabel);
                }

                // Else, continue with target node.
                else
                    ProcessSubGraph(toNode, graph, executionOrder, ref nextLabel);
            }

            // Set output parameters.
            SetOutputArguments(node, ref nextLabel);
        }

        /// <summary>
        /// Get the label of a node. Returns null if the node has no label.
        /// </summary>
        private static string GetLabel(CompilerNode node)
        {
            for (int i = 0; i < node.Children.Count; i++)
            {
                if (node.Children[i].Data.GetOpcode() == BuiltIn.LabelOpcode)
                    return node.Children[i].Data.GetArgument(BuiltIn.LabelNameId);
            }
            return null;
        }

        /// <summary>
        /// Set the label of a node.
        /// </summary>
        private static void SetLabel(CompilerNode node, string value)
        {
            if (node.Children.Count == 0 || node.Children[0].Data.GetOpcode() != BuiltIn.LabelOpcode)
            {
                node.Children.Insert(0, CompilerNodeMaker.GetLabel(node.Data.Set, value));
                return;
            }


            int labelArgumentIndex = node.Data.Set[BuiltIn.LabelOpcode].GetParameterIndex(BuiltIn.LabelNameId);
            node.Children[0].Data.SetArgument(BuiltIn.LabelNameId, value);
        }

        /// <summary>
        /// Set the output arguments of a node. Also adds labels to the connected node(s) if necessary.
        /// </summary>
        private static void SetOutputArguments(CompilerNode node, ref int nextLabel)
        {
            // Collect output data.
            OutputData nodeOutputData = node.GetOutputData();

            // Set arguments.
            for (int i = 0; i < nodeOutputData.ArgumentCount; i++)
            {
                // Get output index.
                int outputIndex = i;
                if (nodeOutputData.HasDefaultOutput)
                    outputIndex++;

                // Add a label to the target node.
                CompilerNode toNode = node.Outputs[outputIndex].ToNode as CompilerNode;
                if (GetLabel(toNode) == null)
                {
                    SetLabel(toNode, nextLabel.ToString());
                    nextLabel++;
                }

                // Set argument.
                SetOutputArgument(nodeOutputData, i, GetLabel(toNode));
            }
        }

        /// <summary>
        /// Set an output argument of a node.
        /// </summary>
        private static void SetOutputArgument(OutputData outputData, int argumentOutputIndex, string value)
        {
            // Find node and argument index.
            var argumentOutput = outputData.GetOutput(argumentOutputIndex);
            Node<NodeData> node = argumentOutput.Node;
            int argumentIndex = argumentOutput.ArgumentIndex;

            // Set argument value.
            node.Data.Instance.Arguments[argumentIndex] = value;
        }
    }
}
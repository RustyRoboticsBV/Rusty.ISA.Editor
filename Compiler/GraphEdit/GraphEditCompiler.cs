using Rusty.Cutscenes;
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
            Graph<NodeData> graph = new();
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
                RootNode<NodeData> fromCompilerNode = graph[i];

                // For each output slot...
                for (int j = 0; j < fromEditorNode.Slots.Count; j++)
                {
                    // Skip empty outputs.
                    if (fromEditorNode.Slots[j].Output == null)
                        continue;

                    // Else, connect nodes.
                    else
                    {
                        CutsceneGraphNode toEditorNode = fromEditorNode.Slots[j].Output.Node;
                        int toNodeIndex = graphEdit.Nodes.IndexOf(toEditorNode);
                        RootNode<NodeData> toCompilerNode = graph[toNodeIndex];

                        fromCompilerNode.ConnectTo(toCompilerNode);
                    }
                }
            }

            // 3. Figure out start nodes.
            int[] startNodes = graph.FindStartNodes();

            // 4. Figure out execution order, set labels, insert go-to's and set output arguments.
            BiDict<int, RootNode<NodeData>> executionOrder = new();
            int nextLabel = 0;
            for (int i = 0; i < startNodes.Length; i++)
            {
                RootNode<NodeData> startNode = graph[startNodes[i]];
                if (executionOrder.ContainsRight(startNode))
                    continue;

                ProcessSubGraph(startNode, graph, executionOrder, ref nextLabel);
            }

            // 5. Compile to code.
            string code = "Opcode";
            int maxParameterCount = 0;
            for (int i = 0; i < graphEdit.InstructionSet.Definitions.Length; i++)
            {
                int parameterCount = graphEdit.InstructionSet.Definitions[i].Parameters.Length;
                if (parameterCount > maxParameterCount)
                    maxParameterCount = parameterCount;
            }
            for (int i = 0; i < maxParameterCount; i++)
            {
                code += ",Arg" + i;
            }

            for (int i = 0; i < executionOrder.Count; i++)
            {
                code += "\n";
                try
                {
                    var node = executionOrder[i];
                    try
                    {
                        code += NodeCompiler.Compile(node);
                    }
                    catch
                    {
                        code += $"{BuiltIn.ErrorOpcode},\"Could not compile node {i}!\"";
                    }
                }
                catch
                {
                    code += $"{BuiltIn.ErrorOpcode},\"Missing node {i}!\"";
                }
            }
            return code;
        }

        /* Private methods. */
        /// <summary>
        /// Helper function for step 4 of the compilation process.
        /// It figures out the execution order of the graph, sets labels, inserts go-to's and sets output arguments.
        /// </summary>
        private static void ProcessSubGraph(RootNode<NodeData> node, Graph<NodeData> graph,
            BiDict<int, RootNode<NodeData>> executionOrder, ref int nextLabel)
        {
            // Add to execution order.
            executionOrder.Add(nextLabel, node);

            // Set label value.
            SetLabel(node, nextLabel.ToString());
            nextLabel++;

            // Continue with output nodes.
            for (int i = 0; i < node.Outputs.Count; i++)
            {
                RootNode<NodeData> toNode = node.Outputs[i].ToNode;

                // If there was no successor node, add an end instead.
                if (toNode == null)
                {
                    // Create end node.
                    RootNode<NodeData> end = CompilerNodeMaker.Create(graph.Data.Set, BuiltIn.EndOpcode);
                    graph.AddNode(end);

                    // Connect it.
                    node.Outputs[i].ConnectTo(end);

                    // Add to execution order.
                    executionOrder.Add(nextLabel, end);

                    // Set the label.
                    SetLabel(end, nextLabel.ToString());
                    nextLabel++;
                }

                // If the target node has been added already, add a go-to instead.
                else if (executionOrder.ContainsRight(toNode))
                {
                    // Create go-to node.
                    RootNode<NodeData> gto = CompilerNodeMaker.Create(graph.Data.Set, BuiltIn.GotoOpcode);
                    graph.AddNode(gto);

                    // Connect it.
                    node.Outputs[i].ConnectTo(gto);
                    gto.ConnectTo(toNode);

                    // Add to execution order.
                    executionOrder.Add(nextLabel, gto);

                    // Set the label.
                    SetLabel(gto, nextLabel.ToString());
                    nextLabel++;

                    // Set output parameter.
                    SetOutputArguments(gto);
                }

                // Else, continue with target node.
                else
                    ProcessSubGraph(toNode, graph, executionOrder, ref nextLabel);
            }

            // Set output parameters.
            SetOutputArguments(node);
        }

        /// <summary>
        /// Get the label of a node.
        /// </summary>
        private static string GetLabel(RootNode<NodeData> node)
        {
            try
            {
                int labelArgumentIndex = node.Data.Set[BuiltIn.LabelOpcode].GetParameterIndex(BuiltIn.LabelNameId);
                return node.Children[0].Data.Instance.Arguments[labelArgumentIndex];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Set the label of a node.
        /// </summary>
        private static void SetLabel(RootNode<NodeData> node, string value)
        {
            try
            {
                int labelArgumentIndex = node.Data.Set[BuiltIn.LabelOpcode].GetParameterIndex(BuiltIn.LabelNameId);
                node.Children[0].Data.Instance.Arguments[labelArgumentIndex] = value;
            }
            catch { }
        }

        /// <summary>
        /// Get the output data of a node.
        /// </summary>
        private static void GetOutputData(Node<NodeData> node, ref OutputData<Node<NodeData>> result)
        {
            // Handle arguments.
            for (int i = 0; i < node.Data.Definition.Parameters.Length; i++)
            {
                if (node.Data.Definition.Parameters[i] is OutputParameter output)
                {
                    if (output.OverrideDefaultOutput)
                        result.HasDefaultOutput = false;
                    result.ArgumentOutputs.Add(new(node, i));
                }
            }

            // Handle child nodes.
            foreach (SubNode<NodeData> sub in node.Children)
            {
                GetOutputData(sub, ref result);
            }
        }

        /// <summary>
        /// Set the output arguments of a node.
        /// </summary>
        private static void SetOutputArguments(RootNode<NodeData> node)
        {
            // Collect output data.
            OutputData<Node<NodeData>> nodeOutputData = new();
            GetOutputData(node, ref nodeOutputData);

            // Set arguments.
            for (int i = 0; i < nodeOutputData.ArgumentOutputs.Count; i++)
            {
                // Get output index.
                int outputIndex = i;
                if (nodeOutputData.HasDefaultOutput)
                    outputIndex++;

                // Set argument.
                SetOutputArgument(nodeOutputData, i, GetLabel(node.Outputs[outputIndex].ToNode));
            }
        }

        /// <summary>
        /// Set an output argument of a node.
        /// </summary>
        private static void SetOutputArgument(OutputData<Node<NodeData>> outputData, int argumentOutputIndex, string value)
        {
            // Find node and argument index.
            var argumentOutput = outputData.ArgumentOutputs[argumentOutputIndex];
            Node<NodeData> node = argumentOutput.Source;
            int argumentIndex = argumentOutput.ArgumentIndex;

            // Set argument value.
            node.Data.Instance.Arguments[argumentIndex] = value;
        }
    }
}
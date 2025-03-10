using Godot;
using Rusty.Graphs;
using Rusty.Maps;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// An utility that compiles a compiler graph into code.
    /// </summary>
    public class GraphCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile a compiler node graph. This inserts gotos and ends into the graph.
        /// </summary>
        public static string Compile(CompilerGraph graph)
        {
            // 1. Add instruction set metadata.
            CompilerNode setNode = MetaSetCompiler.Compile(graph.Data.Set);
            graph.InsertNode(0, setNode);

            // 2. Figure out start nodes.
            int[] startNodes = graph.FindStartNodes();

            // 3. Figure out execution order, set labels, insert goto's and set output arguments.
            BiDict<int, CompilerNode> executionOrder = new();
            int nextLabel = 0;
            for (int i = 0; i < startNodes.Length; i++)
            {
                CompilerNode startNode = graph[startNodes[i]];

                if (startNode.Data.GetOpcode() == BuiltIn.MetadataOpcode)
                {
                    executionOrder.Add(executionOrder.Count, startNode);
                    continue;
                }

                if (executionOrder.ContainsRight(startNode))
                    continue;

                ProcessSubGraph(startNode, graph, executionOrder, ref nextLabel);
            }

            // Print finished graph.
            GD.Print("Compiled graph:\n" + graph);

            // 4. Compile to code.
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
        /// It figures out the execution order of the graph, sets labels, inserts goto's and sets output arguments.
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
                    CompilerNode end = CompilerNodeMaker.GetNode(graph.Data.Set, "", "");
                    end.AddChild(CompilerNodeMaker.GetEnd(graph.Data.Set));
                    end.AddChild(CompilerNodeMaker.GetEndOfGroup(graph.Data.Set));
                    graph.AddNode(end);

                    // Connect it.
                    node.Outputs[i].ConnectTo(end);

                    // Add to execution order.
                    executionOrder.Add(executionOrder.Count, end);
                }

                // If the target node has been added already, add a goto instead.
                else if (executionOrder.ContainsRight(toNode))
                {
                    // Create goto node.
                    CompilerNode @goto = CompilerNodeMaker.GetNode(graph.Data.Set, "", "");
                    @goto.AddChild(CompilerNodeMaker.GetLabel(graph.Data.Set, ""));
                    @goto.AddChild(CompilerNodeMaker.GetEndOfGroup(graph.Data.Set));
                    graph.AddNode(@goto);

                    // Connect it.
                    node.Outputs[i].ConnectTo(@goto);
                    @goto.ConnectTo(toNode);

                    // Add to execution order.
                    executionOrder.Add(executionOrder.Count, @goto);

                    // Set output parameter.
                    SetOutputArguments(@goto, ref nextLabel);
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
                    return node.Children[i].Data.GetArgument(BuiltIn.LabelName);
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


            int labelArgumentIndex = node.Data.Set[BuiltIn.LabelOpcode].GetParameterIndex(BuiltIn.LabelName);
            node.Children[0].Data.SetArgument(BuiltIn.LabelName, value);
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
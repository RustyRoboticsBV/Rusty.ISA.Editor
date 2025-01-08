using System.Collections.Generic;
using Godot;
using Rusty.Cutscenes;
using Rusty.Graphs;
using Rusty.Maps;

namespace Rusty.CutsceneEditor.Compiler
{
    namespace Rusty.Collections
    {
        /// <summary>
        /// A list that uses a dictionary for faster Contains and IndexOf calls at the cost of slower InsertAt calls. May only
        /// contain unique values.
        /// </summary>
        public class ListDict<T>
        {
            /* Public properties. */
            public int Count => Values.Count;

            /* Private properties. */
            private List<T> Values { get; } = new();
            private Dictionary<T, int> Indices { get; } = new();

            /* Constructors. */
            public ListDict() { }

            /* Indexers. */
            public T this[int index] => Values[index];
            public int this[T value] => Indices[value];

            /* Public methods. */
            public bool Contains(T value)
            {
                return Indices.ContainsKey(value);
            }

            public int IndexOf(T value)
            {
                return Indices[value];
            }

            public void Add(T value)
            {
                Indices.Add(value, Values.Count);
                Values.Add(value);
            }

            public void Insert(int index, T value)
            {
                for (int i = index; i < Values.Count; i++)
                {
                    Indices[Values[i]] = i + 1;
                }
                Values.Insert(index, value);
                Indices.Add(value, index);
            }

            public void Remove(T value)
            {
                Values.RemoveAt(Indices[value]);
                Indices.Remove(value);
            }

            public void RemoveAt(int index)
            {
                Indices.Remove(Values[index]);
                Values.RemoveAt(index);
            }

            public void Clear()
            {
                Values.Clear();
                Indices.Clear();
            }
        }

        public class Registry<T1, T2>
        {
            public ListDict<T1> List1 { get; } = new();
            public ListDict<T2> List2 { get; } = new();

            /* Constructors. */
            public Registry() { }

            /* Indexers. */
            public (T1, T2) this[int index] => new(List1[index], List2[index]);
            public int this[T1 item] => List1[item];
            public int this[T2 item] => List2[item];

            /* Public methods. */
            public bool Contains(T1 item)
            {
                return List1.Contains(item);
            }

            public bool Contains(T2 item)
            {
                return List2.Contains(item);
            }

            public int IndexOf(T1 item)
            {
                return List1[item];
            }

            public int IndexOf(T2 item)
            {
                return List2[item];
            }

            public void Add(T1 item1, T2 item2)
            {
                List1.Add(item1);
                List2.Add(item2);
            }

            public void Insert(int index, T1 item1, T2 item2)
            {
                List1.Insert(index, item1);
                List2.Insert(index, item2);
            }

            public void Remove(T1 item)
            {
                List2.RemoveAt(IndexOf(item));
                List1.Remove(item);
            }

            public void Remove(T2 item)
            {
                List1.RemoveAt(IndexOf(item));
                List2.Remove(item);
            }

            public void RemoveAt(int index)
            {
                List1.RemoveAt(index);
                List2.RemoveAt(index);
            }

            public void Clear()
            {
                List1.Clear();
                List2.Clear();
            }
        }
    }

    public abstract class GraphEditCompiler : Compiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile a graph edit into a compiler graph that represents the finished program. It consists of the following steps:]
        /// <br/>1. Create non-connected compiler nodes, one for each graph edit node.
        /// <br/>2. Connect all the compiler nodes, according to connections in the graph edit.
        /// <br/>   2.1 We can insert end nodes while doing this.
        /// <br/>3. Find the start nodes.
        /// <br/>4. Figure out the execution order.
        /// <br/>   4.1 We can set node labels while doing this.
        /// <br/>   4.2 We can insert go-to's while doing this.
        /// <br/>   4.3 We can set output parameter values while doing this.
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

            // 2. Connect nodes & insert ends.
            for (int i = 0; i < graphEdit.Nodes.Count; i++)
            {
                // Get editor & compiler node.
                CutsceneGraphNode fromEditorNode = graphEdit.Nodes[i];
                RootNode<NodeData> fromCompilerNode = graph[i];

                // For each output slot...
                for (int j = 0; j < fromEditorNode.Slots.Count; j++)
                {
                    // Add end instruction if necessary.
                    if (fromEditorNode.Slots[j].Output == null)
                    {
                        RootNode<NodeData> end = CompilerNodeMaker.Create(graphEdit.InstructionSet, BuiltIn.EndOpcode);
                        graph.AddNode(end);

                        fromCompilerNode.ConnectTo(end);
                    }

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
                        code += $"ERR,\"Could not compile node {i}!\"";
                    }
                }
                catch
                {
                    code += $"ERR,\"Missing node {i}!\"";
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

                // If the target node has been added already, add a go-to instead.
                if (executionOrder.ContainsRight(toNode))
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
                    var outputData = GetOutputData(gto);
                    SetOutputArgument(outputData, 0, GetLabel(toNode));
                }

                // Else, continue with target node.
                else
                    ProcessSubGraph(toNode, graph, executionOrder, ref nextLabel);
            }

            // Set output parameters.
            var nodeOutputData = GetOutputData(node);
            for (int i = 0; i < nodeOutputData.ArgumentOutputs.Count; i++)
            {
                int outputIndex = i;
                if (nodeOutputData.HasDefaultOutput)
                    outputIndex++;

                SetOutputArgument(nodeOutputData, i, GetLabel(node.Outputs[outputIndex].ToNode));                                         
            }
        }

        private static void SetLabel(RootNode<NodeData> node, string value)
        {
            try
            {
                int labelArgumentIndex = node.Data.Set[BuiltIn.LabelOpcode].GetParameterIndex(BuiltIn.LabelNameId);
                node.Children[0].Data.Instance.Arguments[labelArgumentIndex] = value;
            }
            catch
            {
                GD.PrintErr("Could not set label!");
            }
        }

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

        private static void SetOutputArgument(OutputData<Node<NodeData>> outputData, int argumentOutputIndex, string value)
        {
            // Find node and argument index.
            var argumentOutput = outputData.ArgumentOutputs[argumentOutputIndex];
            Node<NodeData> node = argumentOutput.Source;
            int argumentIndex = argumentOutput.ArgumentIndex;

            // Set argument value.
            node.Data.Instance.Arguments[argumentIndex] = value;
        }

        private static OutputData<Node<NodeData>> GetOutputData(Node<NodeData> node)
        {
            OutputData<Node<NodeData>> result = new();
            FindOutput(node, ref result);
            return result;
        }

        private static void FindOutput(Node<NodeData> node, ref OutputData<Node<NodeData>> result)
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
                FindOutput(sub, ref result);
            }
        }
    }
}
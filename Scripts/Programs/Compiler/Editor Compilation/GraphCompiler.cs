using System;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A joint element compiler.
/// </summary>
public abstract class GraphCompiler : CompilerTool
{
    /* Public methods. */
    /// <summary>
    /// Compile a graph edit into a compiler graph.
    /// </summary>
    public static RootNode Compile(Ledger ledger)
    {
        // Create graph.
        Graph graph = new();
        BiDict<IGraphElement, RootNode> nodes = new();

        // Compile each program unit into a root node.
        foreach (LedgerItem item in ledger.Items)
        {
            // Compile ledger item.
            RootNode root = null;
            switch (item)
            {
                case LedgerNode node:
                    root = NodeCompiler.Compile(ledger.Set, node.Element, node.Inspector);
                    break;
                case LedgerJoint joint:
                    root = JointCompiler.Compile(ledger.Set, joint.Element, joint.Inspector);
                    break;
                case LedgerComment comment:
                    root = CommentCompiler.Compile(ledger.Set, comment.Element, comment.Inspector);
                    break;
                case LedgerFrame frame:
                    root = FrameCompiler.Compile(ledger.Set, frame.Element, frame.Inspector);
                    break;
            }
            graph.AddNode(root);
            nodes.Add(item.Element, root);

            // Find output data.
            CreateOutputs(root);
        }

        // Connect the compiler nodes according to the graph edit's connections.
        foreach (ElementEdges element in ledger.GraphEdit.Edges)
        {
            foreach (KeyValuePair<int, Edge> edge in element.Edges)
            {
                RootNode from = nodes[edge.Value.FromElement];
                int fromPortIndex = edge.Value.FromPortIndex;
                RootNode to = nodes[edge.Value.ToElement];
                from.ConnectTo(fromPortIndex, to, to.InputCount);
            }
        }

        // Find start nodes.
        int[] startNodes = graph.FindStartNodes();

        // Figure out execution order.
        // Also add labels, insert gotos and ends, and set output arguments.
        BiDict<int, RootNode> executionOrder = new();
        int nextLabel = 0;
        for (int i = 0; i < startNodes.Length; i++)
        {
            RootNode node = graph.GetNodeAt(startNodes[i]);
            ProcessSubGraph(ledger.Set, graph, startNodes, node, executionOrder, ref nextLabel);
        }

        // Wrap in a graph root block.
        RootNode graphNode = MakeRoot(ledger.Set, BuiltIn.GraphOpcode);
        for (int i = 0; i < executionOrder.Count; i++)
        {
            graphNode.AddChild(executionOrder[i]);
        }
        graphNode.AddChild(MakeSub(ledger.Set, BuiltIn.EndOfGroupOpcode));

        return graphNode;
    }

    /* Private methods. */
    /// <summary>
    /// Process a sub-graph. This does a number of things:<br/>
    /// - Figuring out the execution order.<br/>
    /// - Adding labels.<br/>
    /// - Adding end instructions.<br/>
    /// - Inserting goto instructions.<br/>
    /// - Setting output arguments.
    /// </summary>
    private static void ProcessSubGraph(InstructionSet instructionSet, Graph graph, int[] startNodes, RootNode node, BiDict<int, RootNode> executionOrder, ref int nextLabel)
    {
        // Do nothing if the node was already in the execution order.
        if (executionOrder.ContainsRight(node))
            return;

        // Add to execution order.
        executionOrder.Add(executionOrder.Count, node);

        // Add label if necessary.
        if (NeedsLabel(node, IsStartNode(graph, startNodes, node)))
        {
            SetLabel(instructionSet, node, nextLabel.ToString());
            nextLabel++;
        }

        // Continue with output nodes.
        for (int i = 0; i < node.OutputCount; i++)
        {
            OutputPort from = node.GetOutputAt(i);

            // If the output was not connected...
            if (from.To == null || from.To.Node == null)
            {
                // Create end group.
                RootNode endGroup = MakeRoot(instructionSet, BuiltIn.EndGroupOpcode);
                endGroup.AddChild(MakeSub(instructionSet, BuiltIn.EndOpcode));
                endGroup.AddChild(MakeSub(instructionSet, BuiltIn.EndOfGroupOpcode));
                graph.AddNode(endGroup);

                // Connect it.
                from.ConnectTo(endGroup.CreateInput());

                // Add label if necessary.
                if (NeedsLabel(endGroup, false))
                {
                    SetLabel(instructionSet, endGroup, nextLabel.ToString());
                    nextLabel++;
                }

                // Add to execution order.
                executionOrder.Add(executionOrder.Count, endGroup);
            }

            // If the target was already in the execution order...
            else if (executionOrder.ContainsRight(from.To.Node))
            {
                // Create goto group.
                RootNode gotoGroup = MakeRoot(instructionSet, BuiltIn.GotoGroupOpcode);
                gotoGroup.AddChild(MakeSub(instructionSet, BuiltIn.GotoOpcode));
                gotoGroup.AddChild(MakeSub(instructionSet, BuiltIn.EndOfGroupOpcode));
                graph.AddNode(gotoGroup);

                // Insert it in-between the current connection.
                InputPort to = from.To;
                from.ConnectTo(gotoGroup.CreateInput());
                gotoGroup.CreateOutput().ConnectTo(to);

                // Find output data.
                CreateOutputs(gotoGroup);

                // Add labels if necessary.
                if (NeedsLabel(gotoGroup, false))
                {
                    SetLabel(instructionSet, gotoGroup, nextLabel.ToString());
                    nextLabel++;
                }

                // Add to execution order.
                executionOrder.Add(executionOrder.Count, gotoGroup);

                // Set output argument.
                SetOutputArguments(gotoGroup, ref nextLabel);
            }

            // Else, continue with sub-graph.
            else
                ProcessSubGraph(instructionSet, graph, startNodes, from.To.Node, executionOrder, ref nextLabel);
        }

        // Set output arguments.
        SetOutputArguments(node, ref nextLabel);
    }

    /// <summary>
    /// Find all output arguments of a root node and save the node and ID of each in the associated output port.
    /// </summary>
    private static void CreateOutputs(RootNode node)
    {
        // Collect output data.
        OutputArguments outputs = new(node);

        // Ensure the correct number of outputs.
        while (node.OutputCount < outputs.TotalOutputNumber)
        {
            node.CreateOutput();
        }

        // Store references to the output arguments inside of the output ports.
        for (int i = 0; i < outputs.Arguments.Count; i++)
        {
            int portIndex = outputs.UsesDefaultOutput ? i + 1 : i;

            OutputPort output = node.GetOutputAt(portIndex);
            output.OutputParameterNode = outputs.Arguments[portIndex].Node;
            output.OutputParameterID = outputs.Arguments[portIndex].ParameterID;
            output = node.GetOutputAt(portIndex);
        }
    }

    /// <summary>
    /// Set all output arguments of a node hierarchy, depending on the node its root is connected to.
    /// </summary>
    private static void SetOutputArguments(RootNode node, ref int nextLabel)
    {
        for (int i = 0; i < node.OutputCount; i++)
        {
            OutputPort output = node.GetOutputAt(i);
            if (!output.IsDefaultOutput)
            {
                switch (output.OutputParameterNode)
                {
                    case RootNode root:
                        root.SetArgument(output.OutputParameterID, GetLabel(output.To.Node));
                        break;
                    case SubNode sub:
                        sub.SetArgument(output.OutputParameterID, GetLabel(output.To.Node));
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Check if a root node needs a label:<br/>
    /// - If it has more than one inputs.<br/>
    /// - It is a start node and has one or more inputs.<br/>
    /// - If it connects to an output port associated with an output parameter.<br/>
    /// - If it connects to a goto.
    /// </summary>
    private static bool NeedsLabel(RootNode node, bool isStartNode)
    {
        if (node.InputCount > 1)
            return true;

        if (node.InputCount == 1)
        {
            if (isStartNode)
                return true;
            OutputPort from = node.GetInputAt(0).From;
            if (!from.IsDefaultOutput)
                return true;
            if (from.Node.Opcode == BuiltIn.GotoGroupOpcode)
                return true;
        }

        return false;
    }

    /* Private methods. */
    /// <summary>
    /// Get the label of a node.
    /// </summary>
    private static string GetLabel(RootNode node)
    {
        try
        {
            return node.GetChildWith(BuiltIn.LabelOpcode).Data.GetArgument(BuiltIn.LabelName);
        }
        catch
        {
            return "MISSING_LABEL_ERROR";
        }
    }

    /// <summary>
    /// Set the label of a node.
    /// </summary>
    private static void SetLabel(InstructionSet instructionSet, RootNode node, string value)
    {
        // Find label sub-node.
        SubNode label = node.GetChildWith(BuiltIn.LabelOpcode);

        // Or create it if it wasn't found.
        if (label == null)
        {
            label = MakeSub(instructionSet, BuiltIn.LabelOpcode);
            node.InsertChild(0, label);
        }

        // Set label name argument.
        label.SetArgument(BuiltIn.LabelName, value);
    }

    /// <summary>
    /// Check if a node is a start node.
    /// </summary>
    private static bool IsStartNode(Graph graph, int[] startNodes, RootNode node)
    {
        int indexInGraph = graph.IndexOfNode(node);
        return Array.IndexOf(startNodes, indexInGraph) != -1;
    }
}
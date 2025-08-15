using Godot;
using Rusty.Graphs;
using System.Reflection;
using System.Security.Cryptography;

namespace Rusty.ISA.Editor;

/// <summary>
/// A program syntax tree.
/// </summary>
public class SyntaxTree
{
    public RootNode Root { get; private set; }
    public InstructionSet InstructionSet { get; private set; }

    /* Constructors. */
    public SyntaxTree(InstructionSet set, GraphEdit graphEdit, DualDict<IGraphElement, Inspector, Unit> contents)
    {
        InstructionSet = set;

        // Create metadata.
        RootNode metadata = CompilerNodeMaker.MakeRoot(InstructionSet, BuiltIn.MetadataOpcode);

        SubNode checksum = CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.ChecksumOpcode);
        metadata.AddChild(checksum);

        SubNode isa = CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.InstructionSetOpcode);
        metadata.AddChild(isa);
        for (int i = 0; i < set.Count; i++)
        {
            metadata.AddChild(ProcessDefinition(set[i]));
        }
        isa.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOfGroupOpcode));

        metadata.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOfGroupOpcode));

        // Create a program graph.
        Graph graph = new();
        BiDict<IGraphElement, RootNode> nodes = new();

        // Compile each program unit into a root node.
        foreach (Unit unit in contents)
        {
            // Compile program unit
            RootNode node = unit.Compile();
            graph.AddNode(node);
            nodes.Add(unit.Element, node);

            // Find output data.
            FindOutputArguments(node);
        }

        // Connect the compiler nodes according to the graph edit's connections.
        foreach (var element in graphEdit.Edges)
        {
            foreach (var edge in element.Edges)
            {
                RootNode from = nodes[edge.Value.FromElement];
                int fromPortIndex = edge.Value.FromPortIndex;
                RootNode to = nodes[edge.Value.ToElement];
                int toPortIndex = edge.Value.ToPortIndex;
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
            RootNode node = graph.GetNodeAt(startNodes[i]) as RootNode;
            ProcessSubGraph(graph, node, executionOrder, ref nextLabel);
        }

        GD.Print(graph);

        // Wrap in a program node.
        RootNode program = CompilerNodeMaker.MakeRoot(set, BuiltIn.GraphOpcode);
        foreach ((int, RootNode) root in executionOrder)
        {
            program.AddChild(root.Item2);
        }
        program.AddChild(CompilerNodeMaker.MakeSub(set, BuiltIn.EndOfGroupOpcode));

        // Wrap in file node.
        RootNode file = CompilerNodeMaker.MakeRoot(set, BuiltIn.ProgramOpcode);
        file.AddChild(metadata);
        file.AddChild(program);
        file.AddChild(CompilerNodeMaker.MakeSub(set, BuiltIn.EndOfGroupOpcode));

        // Compute checksum.
        checksum.SetArgument(BuiltIn.ChecksumValue, file.GetChecksum());

        Root = file;
        GD.Print(this);
        GD.Print(Compile());
    }

    public SyntaxTree(InstructionSet set, string code)
    {
        // Read CSV table.
        Csv.CsvTable csv = new("code", code);

        // Decompile instruction instances.
        InstructionInstance[] instances = new InstructionInstance[csv.Height];
        for (int i = 0; i < csv.Height; i++)
        {
            InstructionDefinition definition = set[csv[0, i]];
            instances[i] = new(definition);
            for (int j = 0; j < definition.Parameters.Length; j++)
            {
                instances[i].Arguments[j] = csv[j + 1, i];
            }
        }

        // Create syntax tree.
        int current = 0;
        SubNode root = Decompile(set, instances, ref current);
        Root = root.ToRoot();
        Root.StealChildren(root);

        // Evaluate checksum.
        string checksum = Root?.GetChildWith(BuiltIn.MetadataOpcode)?.GetChildWith(BuiltIn.ChecksumOpcode)
            .GetArgument(BuiltIn.ChecksumValue);
        string checksumNew = Root?.GetChecksum();
        if (checksumNew != checksum)
        {
            GD.PrintErr("Loaded program had a wrong checksum! This means the data was either modified or corrupted!");
            GD.PrintErr("File checksum: " + checksum);
            GD.PrintErr("Recalculated checksum: " + checksumNew);
        }
        else
            GD.Print("Checksum result: the data was valid.");
    }

    private SubNode Decompile(InstructionSet set, InstructionInstance[] instances, ref int current)
    {
        // Create node for current instruction.
        InstructionInstance instance = instances[current];

        SubNode node = CompilerNodeMaker.MakeSub(set, instance.Opcode);
        node.Data.Instance = instance;

        // Determine how to proceed.
        current++;
        switch (instance.Opcode)
        {
            // Groups.
            case BuiltIn.ProgramOpcode:
            case BuiltIn.MetadataOpcode:
            case BuiltIn.InstructionSetOpcode:
            case BuiltIn.DefinitionOpcode:
            case BuiltIn.CompileRuleOpcode:
            case BuiltIn.GraphOpcode:
            case BuiltIn.CommentOpcode:
            case BuiltIn.FrameOpcode:
            case BuiltIn.JointOpcode:
            case BuiltIn.NodeOpcode:
            case BuiltIn.InspectorOpcode:
            case BuiltIn.PreInstructionOpcode:
            case BuiltIn.PostInstructionOpcode:
            case BuiltIn.OptionRuleOpcode:
            case BuiltIn.ChoiceRuleOpcode:
            case BuiltIn.TupleRuleOpcode:
            case BuiltIn.ListRuleOpcode:
            case BuiltIn.GotoGroupOpcode:
            case BuiltIn.EndGroupOpcode:
                while (current < instances.Length)
                {
                    SubNode child = Decompile(set, instances, ref current);
                    node.AddChild(child);
                    if (child.Opcode == BuiltIn.EndOfGroupOpcode)
                        break;
                }
                break;

            // Non-groups.
            default:
                break;
        }

        return node;
    }

    /* Public methods. */
    public override string ToString()
    {
        return Root.ToString();
    }

    /// <summary>
    /// Generate code.
    /// </summary>
    public string Compile()
    {
        return Compile(Root);
    }

    /* Private methods. */
    // Graph.
    /// <summary>
    /// Process a sub-graph. This does a number of things:<br/>
    /// - Figuring out the execution order.<br/>
    /// - Adding labels.<br/>
    /// - Adding end instructions.<br/>
    /// - Inserting goto instructions.<br/>
    /// - Setting output arguments.
    /// </summary>
    private void ProcessSubGraph(Graph graph, RootNode node, BiDict<int, RootNode> executionOrder, ref int nextLabel)
    {
        // Do nothing if the node was already in the execution order.
        if (executionOrder.ContainsRight(node))
            return;

        // Add to execution order.
        executionOrder.Add(executionOrder.Count, node);

        // Add label if necessary.
        if (NeedsLabel(node))
        {
            SetLabel(node, nextLabel.ToString());
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
                RootNode endGroup = CompilerNodeMaker.MakeRoot(InstructionSet, BuiltIn.EndGroupOpcode);
                endGroup.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOpcode));
                endGroup.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOfGroupOpcode));
                graph.AddNode(endGroup);

                // Connect it.
                from.ConnectTo(endGroup.CreateInput());

                // Add label if necessary.
                if (NeedsLabel(endGroup))
                {
                    SetLabel(endGroup, nextLabel.ToString());
                    nextLabel++;
                }

                // Add to execution order.
                executionOrder.Add(executionOrder.Count, endGroup);
            }

            // If the target was already in the execution order...
            else if (executionOrder.ContainsRight(from.To.Node))
            {
                // Create goto group.
                RootNode gotoGroup = CompilerNodeMaker.MakeRoot(InstructionSet, BuiltIn.GotoGroupOpcode);
                gotoGroup.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.GotoOpcode));
                gotoGroup.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOfGroupOpcode));
                graph.AddNode(gotoGroup);

                // Insert it in-between the current connection.
                InputPort to = from.To;
                from.ConnectTo(gotoGroup.CreateInput());
                gotoGroup.CreateOutput().ConnectTo(to);

                // Find output data.
                FindOutputArguments(gotoGroup);

                // Add label if necessary.
                if (NeedsLabel(gotoGroup))
                {
                    SetLabel(gotoGroup, nextLabel.ToString());
                    nextLabel++;
                }

                // Add to execution order.
                executionOrder.Add(executionOrder.Count, gotoGroup);

                // Set output argument.
                SetOutputArguments(gotoGroup, ref nextLabel);
            }

            // Else, continue with sub-graph.
            else
                ProcessSubGraph(graph, from.To.Node, executionOrder, ref nextLabel);
        }

        // Set output arguments.
        SetOutputArguments(node, ref nextLabel);
    }

    /// <summary>
    /// Find all output arguments of a root node and save the node and ID of each in the associated output port.
    /// </summary>
    private void FindOutputArguments(RootNode node)
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
    private void SetOutputArguments(RootNode node, ref int nextLabel)
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
    /// - If it connects to an output port associated with an output parameter.<br/>
    /// - If it connects to a goto.
    /// </summary>
    private bool NeedsLabel(RootNode node)
    {
        if (node.InputCount > 1)
            return true;

        if (node.InputCount == 1)
        {
            OutputPort from = node.GetInputAt(0).From;
            if (!from.IsDefaultOutput)
                return true;
            if (from.Node.Opcode == BuiltIn.GotoGroupOpcode)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Get the label of a node.
    /// </summary>
    private string GetLabel(RootNode node)
    {
        return node.GetChildWith(BuiltIn.LabelOpcode).Data.GetArgument(BuiltIn.LabelName);
    }

    /// <summary>
    /// Set the label of a node.
    /// </summary>
    private void SetLabel(RootNode node, string value)
    {
        // Find label sub-node.
        SubNode label = node.GetChildWith(BuiltIn.LabelOpcode);

        // Or create it if it wasn't found.
        if (label == null)
        {
            label = CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.LabelOpcode);
            node.InsertChild(0, label);
        }

        // Set label name argument.
        label.SetArgument(BuiltIn.LabelName, value);
    }

    // Metadata.
    /// <summary>
    /// Generate a sub-node hierarchy for an instruction definition.
    /// </summary>
    private SubNode ProcessDefinition(InstructionDefinition instruction)
    {
        // Instruction definition header.
        SubNode definition = CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.DefinitionOpcode);
        definition.SetArgument(BuiltIn.DefinitionOpcodeParameter, instruction.Opcode);

        // Parameters.
        for (int i = 0; i < instruction.Parameters.Length; i++)
        {
            definition.AddChild(ProcessParameter(instruction.Parameters[i]));
        }

        // Pre-instructions.
        if (instruction.PreInstructions.Length > 0)
        {
            SubNode pre = CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.PreInstructionOpcode);
            foreach (CompileRule rule in instruction.PreInstructions)
            {
                pre.AddChild(ProcessRule(rule));
            }
            pre.AddChild(CompilerNodeMaker.MakeRoot(InstructionSet, BuiltIn.EndOfGroupOpcode));
            definition.AddChild(pre);
        }

        // Post-instructions.
        if (instruction.PostInstructions.Length > 0)
        {
            SubNode post = CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.PostInstructionOpcode);
            foreach (CompileRule rule in instruction.PostInstructions)
            {
                post.AddChild(ProcessRule(rule));
            }
            post.AddChild(CompilerNodeMaker.MakeRoot(InstructionSet, BuiltIn.EndOfGroupOpcode));
            definition.AddChild(post);
        }

        // End-of-group.
        definition.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOfGroupOpcode));

        return definition;
    }

    /// <summary>
    /// Generate a sub-node for a parameter.
    /// </summary>
    private SubNode ProcessParameter(Parameter parameter)
    {
        SubNode definition = CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.ParameterOpcode);
        definition.SetArgument(BuiltIn.ParameterType, parameter.GetType().GetCustomAttribute<XmlClassAttribute>().XmlKeyword);
        definition.SetArgument(BuiltIn.ParameterID, parameter.ID);
        return definition;
    }

    /// <summary>
    /// Generate a sub-node / sub-node hierarchy for a compile rule.
    /// </summary>
    private SubNode ProcessRule(CompileRule rule)
    {
        if (rule is InstructionRule instruction)
        {
            SubNode reference = CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.ReferenceOpcode);
            reference.SetArgument(BuiltIn.ReferenceID, rule.ID);
            reference.SetArgument(BuiltIn.ReferenceOpcodeParameter, instruction.Opcode);
            return reference;
        }
        else
        {
            SubNode definition = CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.CompileRuleOpcode);
            definition.SetArgument(BuiltIn.CompileRuleType, rule.GetType().GetCustomAttribute<XmlClassAttribute>().XmlKeyword);
            definition.SetArgument(BuiltIn.CompileRuleID, rule.ID);
            switch (rule)
            {
                case OptionRule option:
                    if (option.Type != null)
                        definition.AddChild(ProcessRule(option.Type));
                    break;
                case ChoiceRule choice:
                    foreach (CompileRule type in choice.Types)
                    {
                        if (type != null)
                            definition.AddChild(ProcessRule(type));
                    }
                    break;
                case TupleRule tuple:
                    foreach (CompileRule type in tuple.Types)
                    {
                        if (type != null)
                            definition.AddChild(ProcessRule(type));
                    }
                    break;
                case ListRule list:
                    if (list.Type != null)
                        definition.AddChild(ProcessRule(list.Type));
                    break;
            }
            definition.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOfGroupOpcode));
            return definition;
        }
    }

    // Compilation.
    /// <summary>
    /// Compile a node.
    /// </summary>
    private static string Compile(INode node)
    {
        string code = Compile(node.Data is NodeData data ? data : null);
        for (int i = 0; i < node.ChildCount; i++)
        {
            code += $"\n{Compile(node.GetChildAt(i))}";
        }
        return code;
    }

    /// <summary>
    /// Compile node data.
    /// </summary>
    private static string Compile(NodeData data)
    {
        if (data == null)
            return "";

        string code = data.Instance.Opcode;
        for (int i = 0; i < data.Instance.Arguments.Length; i++)
        {
            // Get argument.
            string arg = data.Instance.Arguments[i];

            // Make argument CSV-safe:
            // - Replace " with "".
            // - Enclose in " if containing a , or " character.
            // - Replace \n with \\n.
            // - Replace line-breaks with \n.
            arg = arg.Replace("\"", "\"\"");
            if (arg.Contains(',') || arg.Contains('"'))
                arg = '"' + arg + '"';
            arg = arg.Replace("\\n", "\\\\n").Replace("\n", "\\n");

            // Add to code.
            code += $",{arg}";
        }

        return code;
    }
}
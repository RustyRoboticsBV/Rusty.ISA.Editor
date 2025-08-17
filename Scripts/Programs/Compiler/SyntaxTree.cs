using Godot;
using System.Collections.Generic;
using System.Reflection;
using Rusty.Csv;
using Rusty.Graphs;

namespace Rusty.ISA.Editor;

/// <summary>
/// A program syntax tree.
/// </summary>
public class SyntaxTree
{
    public RootNode Root { get; private set; }
    public InstructionSet InstructionSet { get; private set; }

    /* Constructors. */
    public SyntaxTree(Ledger ledger)
    {
        InstructionSet = ledger.Set;

        // Create metadata.
        RootNode metadata = CompilerNodeMaker.MakeRoot(InstructionSet, BuiltIn.MetadataOpcode);

        SubNode checksum = CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.ChecksumOpcode);
        metadata.AddChild(checksum);

        SubNode isa = CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.InstructionSetOpcode);
        for (int i = 0; i < InstructionSet.Count; i++)
        {
            isa.AddChild(ProcessDefinition(InstructionSet[i]));
        }
        isa.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOfGroupOpcode));
        metadata.AddChild(isa);

        metadata.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOfGroupOpcode));

        // Compile graph.
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
                try
                {
                    RootNode from2 = nodes[edge.Value.FromElement];
                }
                catch {
                    GD.Print("OH FUCK " + edge.Value.FromElement.Name + " WAS NOT FOUND");
                }
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
            ProcessSubGraph(graph, node, executionOrder, ref nextLabel);
        }

        GD.Print(graph);

        // Wrap in a program node.
        RootNode program = CompilerNodeMaker.MakeRoot(InstructionSet, BuiltIn.GraphOpcode);
        foreach ((int, RootNode) root in executionOrder)
        {
            program.AddChild(root.Item2);
        }
        program.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOfGroupOpcode));

        // Wrap in file node.
        RootNode file = CompilerNodeMaker.MakeRoot(InstructionSet, BuiltIn.ProgramOpcode);
        file.AddChild(metadata);
        file.AddChild(program);
        file.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOfGroupOpcode));

        // Compute checksum.
        checksum.SetArgument(BuiltIn.ChecksumValue, file.GetChecksum());

        Root = file;
        GD.Print(this);
        GD.Print(Compile());
    }

    public SyntaxTree(InstructionSet set, string code)
    {
        InstructionSet = set;

        // Read CSV table.
        CsvTable csv = new("code", code);

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

    /// <summary>
    /// Spawn graph elements from this syntax tree into a graph edit, create inspectors for each and write arguments into the
    /// inspectors.
    /// </summary>
    public void Decompile(Ledger ledger)
    {
        SubNode programNode = Root?.GetChildWith(BuiltIn.GraphOpcode);

        // Create graph.
        Graph graph = new();
        Dictionary<string, RootNode> labeledNodes = new();
        for (int i = 0; i < programNode.ChildCount; i++)
        {
            // Find element node.
            SubNode child = programNode.GetChildAt(i);

            // Ignore the end-of-group.
            if (child.Opcode == BuiltIn.EndOfGroupOpcode)
                continue;

            // Convert it to a root node.
            RootNode root = child.ToRoot();
            root.StealChildren(child);
            graph.AddNode(root);

            // Find output argument data.
            if (child.Opcode != BuiltIn.EndGroupOpcode)
                CreateOutputs(root);

            // Find label.
            if (HasLabel(root))
                labeledNodes.Add(GetLabel(root), root);
        }

        // Connect argument outputs.
        for (int i = 0; i < graph.NodeCount; i++)
        {
            RootNode node = graph.GetNodeAt(i);
            if (node.Opcode == BuiltIn.EndGroupOpcode)
                continue;
            for (int j = 0; j < node.OutputCount; j++)
            {
                OutputPort output = node.GetOutputAt(j);
                if (output.IsDefaultOutput && i < graph.NodeCount - 1)
                    output.ConnectTo(graph.GetNodeAt(i + 1));
                else
                {
                    string label = null;
                    switch (output.OutputParameterNode)
                    {
                        case RootNode root:
                            label = root.GetArgument(output.OutputParameterID);
                            break;
                        case SubNode sub:
                            label = sub.GetArgument(output.OutputParameterID);
                            break;
                        default:
                            break;
                    }
                    if (label != null)
                    {
                        RootNode to = labeledNodes[label];
                        output.ConnectTo(to.CreateInput());
                    }
                }
            }
        }

        // Remove gotos and ends.
        for (int i = graph.NodeCount - 1; i >= 0; i--)
        {
            RootNode node = graph.GetNodeAt(i);
            if (node.Opcode == BuiltIn.EndGroupOpcode)
                node.Remove();
            else if (node.Opcode == BuiltIn.GotoGroupOpcode)
                node.Dissolve();
        }

        GD.Print(graph);

        // Spawn objects.
        ledger.Clear();
        Dictionary<RootNode, LedgerItem> items = new();
        for (int i = 0; i < graph.NodeCount; i++)
        {
            RootNode node = graph.GetNodeAt(i);
            switch (node.Opcode)
            {
                case BuiltIn.NodeOpcode:
                    items.Add(node, SpawnNode(ledger, node));
                    break;
                case BuiltIn.JointOpcode:
                    items.Add(node, SpawnJoint(ledger, node));
                    break;
                case BuiltIn.CommentOpcode:
                    items.Add(node, SpawnComment(ledger, node));
                    break;
                case BuiltIn.FrameOpcode:
                    items.Add(node, SpawnFrame(ledger, node));
                    break;
            }
        }

        // Copy connections to graph edit.
        foreach (var item in items)
        {
            for (int i = 0; i < item.Key.OutputCount; i++)
            {
                RootNode fromNode = item.Key;
                RootNode toNode = fromNode.GetOutputAt(i).To?.Node;
                if (toNode != null)
                    ledger.ConnectElements(items[fromNode], i, items[toNode]);
            }
        }

        // Clear the syntax tree.
        Root = null;
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
                CreateOutputs(gotoGroup);

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
    private void CreateOutputs(RootNode node)
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
    /// Get whether or not a node has a label.
    /// </summary>
    private bool HasLabel(RootNode node)
    {
        return node.GetChildWith(BuiltIn.LabelOpcode) != null;
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
        definition.SetArgument(BuiltIn.DefinitionEditorOnly, instruction.Implementation == null);

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

    // Decompilation.
    /// <summary>
    /// Decompile a subset of a list of instruction instances.
    /// </summary>
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

    private LedgerItem SpawnNode(Ledger ledger, RootNode root)
    {
        // Find inspector instruction.
        SubNode inspector = root.GetChildWith(BuiltIn.InspectorOpcode);

        // Find main instruction.
        SubNode instruction = null;
        for (int j = 0; j < inspector.ChildCount; j++)
        {
            SubNode child = inspector.GetChildAt(j);
            if (child.Opcode != BuiltIn.PreInstructionOpcode && child.Opcode != BuiltIn.PostInstructionOpcode)
            {
                instruction = child;
                break;
            }
        }

        // Spawn node.
        int.TryParse(root.GetArgument(BuiltIn.NodeX), out int nodeX);
        int.TryParse(root.GetArgument(BuiltIn.NodeY), out int nodeY);
        LedgerNode item = ledger.CreateElement(InstructionSet[instruction.Opcode], new(nodeX, nodeY)) as LedgerNode;

        // Copy start point.
        SubNode startPoint = root.GetChildWith(BuiltIn.BeginOpcode);
        if (startPoint != null)
        {
            ToggleTextField startPointField = item.Inspector.GetStartPointField();
            startPointField.Checked = true;
            startPointField.Value = startPoint.GetArgument(BuiltIn.BeginOpcode);
        }

        // Write node state to inspector.
        ApplyToInspector(inspector, item.Inspector.GetInstructionInspector());

        return item;
    }

    private void ApplyToInspector(SubNode node, InstructionInspector inspector)
    {
        // Find main instruction, pre-instructions and post-instructions.
        SubNode main = null;
        SubNode pre = null;
        SubNode post = null;
        for (int i = 0; i < node.ChildCount; i++)
        {
            SubNode child = node.GetChildAt(i);
            if (child.Opcode == BuiltIn.PreInstructionOpcode)
                pre = child;
            else if (child.Opcode == BuiltIn.PostInstructionOpcode)
                post = child;
            else
                main = child;
        }

        // Write arguments to inspector.
        for (int i = 0; i < main.Data.Definition.Parameters.Length && i < main.Data.Instance.Arguments.Length; i++)
        {
            string id = main.Data.Definition.Parameters[i].ID;
            string value = main.Data.Instance.Arguments[i];
            inspector.SetParameterField(id, value);
        }

        // Handle pre-instructions.
        if (pre != null)
        {
            for (int i = 0; i < pre.ChildCount && i < inspector.Definition.PreInstructions.Length; i++)
            {
                string id = inspector.Definition.PreInstructions[i].ID;
                SubNode ruleNode = pre.GetChildAt(i);
                RuleInspector ruleInspector = inspector.GetPreInstruction(id);
                ApplyToInspector(ruleNode, ruleInspector);
            }
        }

        // Handle post-instructions.
        if (post != null)
        {
            for (int i = 0; i < post.ChildCount && i < inspector.Definition.PostInstructions.Length; i++)
            {
                string id = inspector.Definition.PostInstructions[i].ID;
                SubNode ruleNode = post.GetChildAt(i);
                RuleInspector ruleInspector = inspector.GetPostInstruction(id);
                ApplyToInspector(ruleNode, ruleInspector);
            }
        }
    }

    private void ApplyToInspector(SubNode node, RuleInspector inspector)
    {
        switch (inspector)
        {
            case InstructionRuleInspector instruction:
                ApplyToInspector(node, instruction);
                break;
            case OptionRuleInspector option:
                ApplyToInspector(node, option);
                break;
            case ChoiceRuleInspector choice:
                ApplyToInspector(node, choice);
                break;
            case TupleRuleInspector tuple:
                ApplyToInspector(node, tuple);
                break;
            case ListRuleInspector list:
                ApplyToInspector(node, list);
                break;
        }
    }

    private void ApplyToInspector(SubNode node, InstructionRuleInspector inspector)
    {
        ApplyToInspector(node, inspector.GetInstructionInspector());
    }

    private void ApplyToInspector(SubNode node, OptionRuleInspector inspector)
    {
        if (node.ChildCount > 1)
        {
            SubNode childNode = node.GetChildAt(0);
            RuleInspector childInspector = inspector.GetChildRule();
            ApplyToInspector(childNode, childInspector);
        }
    }

    private void ApplyToInspector(SubNode node, ChoiceRuleInspector inspector)
    {
        if (node.ChildCount > 1 && int.TryParse(node.GetArgument(BuiltIn.ChoiceRuleSelected), out int selected))
        {
            inspector.SetSelectedIndex(selected);

            SubNode childNode = node.GetChildAt(0);
            RuleInspector childInspector = inspector.GetSelectedElement();
            ApplyToInspector(childNode, childInspector);
        }
    }

    private void ApplyToInspector(SubNode node, TupleRuleInspector inspector)
    {
        for (int i = 0; i < node.ChildCount - 1; i++)
        {
            SubNode childNode = node.GetChildAt(i);
            RuleInspector childInspector = inspector.GetElementInspector(i);
            ApplyToInspector(childNode, childInspector);
        }
    }

    private void ApplyToInspector(SubNode node, ListRuleInspector inspector)
    {
        for (int i = 0; i < node.ChildCount - 1; i++)
        {
            inspector.AddElement();

            SubNode childNode = node.GetChildAt(i);
            RuleInspector childInspector = inspector.GetElementInspector(i);
            ApplyToInspector(childNode, childInspector);
        }
    }

    /// <summary>
    /// Spawn a joint from a compiler node.
    /// </summary>
    private LedgerItem SpawnJoint(Ledger ledger, RootNode root)
    {
        int.TryParse(root.GetArgument(BuiltIn.JointX), out int jointX);
        int.TryParse(root.GetArgument(BuiltIn.JointY), out int jointY);
        return ledger.CreateElement(InstructionSet[root.Opcode], new(jointX, jointY));
    }

    /// <summary>
    /// Spawn a comment from a compiler node.
    /// </summary>
    private LedgerItem SpawnComment(Ledger ledger, RootNode root)
    {
        int.TryParse(root.GetArgument(BuiltIn.CommentX), out int commentX);
        int.TryParse(root.GetArgument(BuiltIn.CommentY), out int commentY);
        return ledger.CreateElement(InstructionSet[root.Opcode], new(commentX, commentY));
    }

    /// <summary>
    /// Spawn a frame from a compiler node.
    /// </summary>
    private LedgerItem SpawnFrame(Ledger ledger, RootNode root)
    {
        int.TryParse(root.GetArgument(BuiltIn.FrameX), out int frameX);
        int.TryParse(root.GetArgument(BuiltIn.FrameY), out int frameY);
        return ledger.CreateElement(InstructionSet[root.Opcode], new(frameX, frameY));
    }
}
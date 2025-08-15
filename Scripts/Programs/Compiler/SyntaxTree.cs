using Godot;
using System.Reflection;
using Rusty.Graphs;

namespace Rusty.ISA.Editor;

/// <summary>
/// A compiler graph.
/// </summary>
public class SyntaxTree : Graphs.Graph
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
            RootNode node = unit.Compile();
            graph.AddNode(node);
            nodes.Add(unit.Element, node);
        }

        // Connect the compiler nodes according to the graph edit's connections.
        foreach (var element in graphEdit.Edges)
        {
            foreach (var port in element)
            {
                RootNode from = nodes[element.Element];
                RootNode to = nodes[port.Element];
                from.ConnectTo(from.OutputCount, to, port.PortIndex);
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
        GD.Print(Serialize());
    }

    /* Public methods. */
    public override string ToString()
    {
        return Root.ToString();
    }


    public string Serialize()
    {
        return Serialize(Root);
    }

    /* Private methods. */
    private void ProcessSubGraph(Graph graph, RootNode node, BiDict<int, RootNode> executionOrder, ref int nextLabel)
    {
        // Do nothing if the node was already in the execution order.
        if (executionOrder.ContainsRight(node))
            return;

        // Add to execution order.
        executionOrder.Add(executionOrder.Count, node);

        // Continue with output nodes.
        for (int i = 0; i < node.OutputCount; i++)
        {
            IOutputPort from = node.GetOutputAt(i);

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

                // Add to execution order.
                executionOrder.Add(executionOrder.Count, endGroup);
            }

            // If the target was already in the execution order...
            else if (executionOrder.ContainsRight(from.To.Node as RootNode))
            {
                // Create goto group.
                RootNode gotoGroup = CompilerNodeMaker.MakeRoot(InstructionSet, BuiltIn.GotoGroupOpcode);
                gotoGroup.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.GotoOpcode));
                gotoGroup.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOfGroupOpcode));
                graph.AddNode(gotoGroup);

                // Insert it in-between the current connection.
                gotoGroup.CreateOutput().ConnectTo(from.To);
                from.ConnectTo(gotoGroup.CreateInput());

                // Add to execution order.
                executionOrder.Add(executionOrder.Count, gotoGroup);

                // Set output argument.
                SetOutputArguments(gotoGroup, ref nextLabel);
            }

            // Else, continue with sub-graph.
            else
                ProcessSubGraph(graph, from.To.Node as RootNode, executionOrder, ref nextLabel);
        }

        // Set output arguments.
        SetOutputArguments(node, ref nextLabel);
    }

    private void SetOutputArguments(RootNode node, ref int nextLabel)
    {
        OutputArguments outputs = node.GetOutputArguments();

        for (int i = 0; i < outputs.Arguments.Count; i++)
        {
            // Get output port index.
            int portIndex = outputs.GetOutputPortIndex(i);

            // Get label value.
            // Add a label if the target node did not have one yet.
            RootNode toNode = node.GetOutputAt(portIndex).To.Node as RootNode;
            SubNode label = GetLabel(toNode);
            if (label == null)
            {
                SetLabel(toNode, nextLabel.ToString());
                nextLabel++;
            }
        }
    }

    private SubNode GetLabel(RootNode node)
    {
        for (int i = 0; i < node.ChildCount; i++)
        {
            if (node.GetChildAt(i) is SubNode child && child.Opcode == BuiltIn.LabelOpcode)
                return child;
        }
        return null;
    }

    private void SetLabel(RootNode node, string value)
    {
        // Find label sub-node.
        SubNode label = null;
        if (node.ChildCount > 0 && node.GetChildAt(0) is SubNode child && child.Opcode == BuiltIn.LabelOpcode)
            label = child;

        // Or create it if it wasn't found.
        else
        {
            label = CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.LabelOpcode);
            node.InsertChild(0, label);
            label = node.GetChildAt(0) as SubNode;
        }

        // Set label name argument.
        label.SetArgument(BuiltIn.LabelName, value);
    }

    private string Serialize(INode node)
    {
        string code = Serialize(node.Data is NodeData data ? data : null);
        for (int i = 0; i < node.ChildCount; i++)
        {
            code += $"\n{Serialize(node.GetChildAt(i))}";
        }
        return code;
    }

    private string Serialize(NodeData data)
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

    // Metadata.
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

    private SubNode ProcessParameter(Parameter parameter)
    {
        SubNode definition = CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.ParameterOpcode);
        definition.SetArgument(BuiltIn.ParameterType, parameter.GetType().GetCustomAttribute<XmlClassAttribute>().XmlKeyword);
        definition.SetArgument(BuiltIn.ParameterID, parameter.ID);
        return definition;
    }

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
}
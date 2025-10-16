using System.Collections.Generic;
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
        Root = ProgramCompiler.Compile(ledger);
    }

    public SyntaxTree(InstructionSet set, string code)
    {
        InstructionSet = set;
        Root = Parser.Parse(set, code);
    }

    /* Public methods. */
    public override string ToString()
    {
        return Root != null ? Root.ToString() : "";
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

        // Spawn objects.
        ledger.Clear();
        Dictionary<RootNode, LedgerItem> items = new();
        Dictionary<string, LedgerFrame> frames = new();
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
                    var frame = SpawnFrame(ledger, node) as LedgerFrame;
                    items.Add(node, frame);
                    frames.Add(node.GetArgument(BuiltIn.FrameID), frame);
                    break;
            }
        }

        // Copy connections to graph edit and add to frames.
        foreach (var item in items)
        {
            // Connections.
            for (int i = 0; i < item.Key.OutputCount; i++)
            {
                RootNode fromNode = item.Key;
                RootNode toNode = fromNode.GetOutputAt(i).To?.Node;
                if (toNode != null)
                    ledger.ConnectElements(items[fromNode], i, items[toNode]);
            }

            // Add to frame.
            SubNode member = item.Key.GetChildWith(BuiltIn.FrameMemberOpcode);
            if (member != null)
            {
                string frameID = member.GetArgument(BuiltIn.FrameMemberID);
                try
                {
                    frames[frameID].Element.AddElement(item.Value.Element);
                }
                catch
                {
                    Log.Error($"Cannot find frame with ID '{frameID}'.");
                }
            }
        }

        // Clear the syntax tree.
        Root = null;
    }

    /* Private methods. */
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
            startPointField.Value = startPoint.GetArgument(BuiltIn.BeginName);
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
        for (int i = 0; i < node.ChildCount - 1; i++)
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
        // Spawn comment at (x, y).
        int.TryParse(root.GetArgument(BuiltIn.CommentX), out int commentX);
        int.TryParse(root.GetArgument(BuiltIn.CommentY), out int commentY);
        var comment = ledger.CreateElement(InstructionSet[root.Opcode], new(commentX, commentY)) as LedgerComment;

        // Set text.
        comment.Inspector.GetTextField().Value = root.GetArgument(BuiltIn.CommentText);

        return comment;
    }

    /// <summary>
    /// Spawn a frame from a compiler node.
    /// </summary>
    private LedgerItem SpawnFrame(Ledger ledger, RootNode root)
    {
        // Spawn frame at (x, y).
        int.TryParse(root.GetArgument(BuiltIn.FrameX), out int frameX);
        int.TryParse(root.GetArgument(BuiltIn.FrameY), out int frameY);
        var frame = ledger.CreateElement(InstructionSet[root.Opcode], new(frameX, frameY)) as LedgerFrame;

        // Set title & color.
        frame.Inspector.GetTitleField().Value = root.GetArgument(BuiltIn.FrameTitle);
        frame.Inspector.GetColorField().Value = StringUtility.ParseColor(root.GetArgument(BuiltIn.FrameColor));

        return frame;
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
}
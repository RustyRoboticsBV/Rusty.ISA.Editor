using Godot;
using Rusty.Graphs;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Rusty.ISA.Editor;

[GlobalClass]
public partial class ProgramEditor : MarginContainer
{
    /* Public properties. */

    /* Private properties. */
    private InstructionSet InstructionSet { get; set; }

    private Button CopyButton { get; set; }
    private InspectorWindow InspectorWindow { get; set; }
    private GraphEdit GraphEdit { get; set; }
    private ContextMenu ContextMenu { get; set; }

    private DualDict<IGraphElement, Inspector, Unit> Contents { get; } = new();
    private int NextFrameID { get; set; } = 0;

    /* Constructors. */
    public ProgramEditor(InstructionSet set)
    {
        InstructionSet = set;

        MouseFilter = MouseFilterEnum.Pass;

        // Add background.
        ColorRect background = new();
        background.Color = new(0.5f, 0.5f, 0.5f);
        AddChild(background);

        // Add vertical box.
        VBoxContainer vbox = new();
        AddChild(vbox);

        // Add buttons.
        CopyButton = new();
        CopyButton.Text = "Copy";
        CopyButton.Pressed += OnPressedCopy;
        vbox.AddChild(CopyButton);

        // Add inspector / resizer / graph hbox.
        HBoxContainer hbox = new();
        hbox.SizeFlagsVertical = SizeFlags.ExpandFill;
        vbox.AddChild(hbox);

        // Create inspector window.
        InspectorWindow = new();
        InspectorWindow.SizeFlagsHorizontal = SizeFlags.Fill;
        hbox.AddChild(InspectorWindow);
        InspectorWindow.Name = "Inspector";

        // Create resizer.
        Resizer resizer = new();
        resizer.Target = InspectorWindow;
        hbox.AddChild(resizer);
        resizer.Name = "Resizer";

        // Create graph.
        GraphEdit = new();
        GraphEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        GraphEdit.CustomMinimumSize = new(256f, 256f);
        GraphEdit.SelectedElement += OnSelectedGraphElement;
        GraphEdit.DeselectedElement += OnDeselectedGraphElement;
        GraphEdit.DeletedElement += OnDeletedGraphElement;
        GraphEdit.RightClicked += OnRightClickedGraph;
        hbox.AddChild(GraphEdit);
        GraphEdit.Name = "GraphEdit";

        // Create context menu.
        ContextMenu = new();
        ContextMenu.UpdateInstructionSet(InstructionSet);
        ContextMenu.SelectedItem += OnMustSpawn;
        AddChild(ContextMenu);
    }

    /* Private methods. */
    private void OnPressedCopy()
    {
        // Create a graph.
        Graph graph = new();
        BiDict<IGraphElement, RootNode> nodes = new();

        // Create metadata.
        RootNode metadata = CompilerNodeMaker.MakeRoot(InstructionSet, BuiltIn.MetadataOpcode);
        metadata.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.ChecksumOpcode));
        metadata.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOfGroupOpcode));

        // Compile each program unit into a root node.
        foreach (Unit unit in Contents)
        {
            RootNode node = unit.Compile();
            graph.AddNode(node);
            nodes.Add(unit.Element, node);
        }

        // Connect the compiler nodes according to the graph edit's connections.
        foreach (var element in GraphEdit.Edges)
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
        RootNode program = CompilerNodeMaker.MakeRoot(InstructionSet, BuiltIn.ProgramOpcode);
        foreach ((int, RootNode) root in executionOrder)
        {
            program.AddChild(root.Item2);
        }
        program.AddChild(CompilerNodeMaker.MakeSub(InstructionSet, BuiltIn.EndOfGroupOpcode));

        // Finish graph.
        graph.AddNode(metadata);
        graph.AddNode(program);
    }

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
        GD.Print(outputs);
        GD.Print("Outputs: " + node.OutputCount);

        for (int i = 0; i < outputs.Arguments.Count; i++)
        {
            // Get output port index.
            int portIndex = outputs.GetOutputPortIndex(i);
            GD.Print(portIndex);

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
            GD.Print(label);
        }

        // Set label name argument.
        GD.Print(label);
        GD.Print("Writing label " + value + " to " + label.Data);
        label.SetArgument(BuiltIn.LabelName, value);
    }

    private void OnRightClickedGraph()
    {
        ContextMenu.Position = (Vector2I)GetGlobalMousePosition();
        ContextMenu.Show();
    }

    private void OnMustSpawn(InstructionDefinition definition)
    {
        // Get spawn position.
        Vector2 globalSpawnPosition = ContextMenu.Position;
        Vector2 spawnPosition = GraphEdit.GetPositionOffsetFromGlobalPosition(globalSpawnPosition);
        int spawnX = (int)spawnPosition.X;
        int spawnY = (int)spawnPosition.Y;
        if (GraphEdit.SnappingEnabled)
        {
            float snap = GraphEdit.SnappingDistance;
            spawnX = (int)(Mathf.Floor(spawnX / snap) * snap);
            spawnY = (int)(Mathf.Floor(spawnY / snap) * snap);
        }

        // Create inspector.
        Inspector inspector = ElementInspectorFactory.Create(InstructionSet, definition);

        // Spawn element.
        IGraphElement element = null;
        switch (definition.Opcode)
        {
            case BuiltIn.JointOpcode:
                GraphJoint joint = GraphEdit.AddJoint(spawnX, spawnY);
                joint.BgColor = definition.EditorNode.MainColor;
                element = joint;
                break;
            case BuiltIn.CommentOpcode:
                GraphComment comment = GraphEdit.AddComment(spawnX, spawnY);
                comment.CommentText = GetParameter<MultilineParameter>(definition, BuiltIn.CommentText).DefaultValue;
                comment.BgColor = definition.EditorNode.MainColor;
                comment.TextColor = definition.EditorNode.TextColor;
                element = comment;
                break;
            case BuiltIn.FrameOpcode:
                GraphFrame frame = GraphEdit.AddFrame(spawnX, spawnY);
                frame.ID = NextFrameID;
                frame.Title = GetParameter<TextlineParameter>(definition, BuiltIn.FrameTitle).DefaultValue;
                frame.TintColor = GetParameter<ColorParameter>(definition, BuiltIn.FrameColor).DefaultValue;
                NextFrameID++;
                element = frame;
                break;
            default:
                GraphNode node = GraphEdit.AddNode(spawnX, spawnY);
                node.TitleText = definition.DisplayName;
                node.TitleIcon = definition.Icon;
                node.TitleColor = definition.EditorNode.MainColor;
                element = node;
                break;
        }

        // Create unit.
        Unit unit = null;
        switch (definition.Opcode)
        {
            case BuiltIn.JointOpcode:
                unit = new JointUnit(InstructionSet, definition.Opcode, element as GraphJoint, inspector);
                break;
            case BuiltIn.CommentOpcode:
                unit = new CommentUnit(InstructionSet, definition.Opcode, element as GraphComment, inspector);
                break;
            case BuiltIn.FrameOpcode:
                unit = new FrameUnit(InstructionSet, definition.Opcode, element as GraphFrame, inspector);
                break;
            default:
                unit = new NodeUnit(InstructionSet, definition.Opcode, element as GraphNode, inspector);
                break;
        }

        // Add contents.
        Contents.Add(element, inspector, unit);
    }

    private void OnSelectedGraphElement(IGraphElement element)
    {
        if (element is GraphJoint)
            return;

        // Retrieve element's inspector.
        Inspector inspector = Contents[element].Inspector;

        // Add element's inspector to inspector window.
        InspectorWindow.Add(inspector);
    }

    private void OnDeselectedGraphElement(IGraphElement element)
    {
        if (element is GraphJoint)
            return;

        // Retrieve element's inspector.
        Inspector inspector = Contents[element].Inspector;

        // Add element's inspector to inspector window.
        InspectorWindow.Remove(inspector);
    }

    private void OnDeletedGraphElement(IGraphElement element)
    {
        if (element is GraphJoint)
            return;

        // Invoke deselection handler.
        OnDeselectedGraphElement(element);

        // Delete inspector.
        Contents.Remove(element);
    }

    private static T GetParameter<T>(InstructionDefinition definition, string id) where T : Parameter
    {
        return definition.GetParameter(id) as T;
    }
}
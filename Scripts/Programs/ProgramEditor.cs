using Godot;
using System.Collections.Generic;

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
        // Compile each program unit into a root node.
        foreach (Unit unit in Contents)
        {
            var node = unit.Compile();
            GD.Print(node);
        }
    }

    private void OnRightClickedGraph()
    {
        ContextMenu.Position = (Vector2I)GetGlobalMousePosition();
        ContextMenu.Show();
    }

    private void OnMustSpawn(InstructionDefinition definition)
    {
        // Get spawn position.
        Vector2 spawnPosition = GraphEdit.GetPositionOffsetFromGlobalPosition(ContextMenu.Position);
        int spawnX = (int)spawnPosition.X;
        int spawnY = (int)spawnPosition.Y;

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
using Godot;
using Godot.Collections;

namespace Rusty.ISA.Editor;

[GlobalClass]
public partial class ProgramEditor : MarginContainer
{
    /* Public properties. */

    /* Private properties. */
    private InstructionSet InstructionSet { get; set; }

    private InspectorWindow InspectorWindow { get; set; }
    private GraphEdit GraphEdit { get; set; }
    private ContextMenu ContextMenu { get; set; }

    private Dictionary<StringName, Control> Inspectors { get; } = new();

    /* Constructors. */
    public ProgramEditor(InstructionSet set)
    {
        InstructionSet = set;

        MouseFilter = MouseFilterEnum.Pass;

        // Add background.
        ColorRect background = new();
        background.Color = new(0.5f, 0.5f, 0.5f);
        AddChild(background);

        // Add inspector / resizer / graph hbox.
        HBoxContainer hbox = new();
        AddChild(hbox);

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
    private void OnRightClickedGraph()
    {
        ContextMenu.Position = (Vector2I)GetGlobalMousePosition();
        ContextMenu.Show();
    }

    private void OnMustSpawn(InstructionDefinition definition)
    {
        // Get spawn position.
        Vector2 spawnPosition = GraphEdit.GetMousePosition();
        int spawnX = (int)spawnPosition.X;
        int spawnY = (int)spawnPosition.Y;

        // Spawn element.
        Node element = null;
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
                frame.Title = GetParameter<TextlineParameter>(definition, BuiltIn.FrameTitle).DefaultValue;
                frame.TintColor = GetParameter<ColorParameter>(definition, BuiltIn.FrameColor).DefaultValue;
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

        // Add inspector.
        if (definition.Opcode != BuiltIn.JointOpcode)
        {
            Inspector nodeInspector = ElementInspectorFactory.Create(InstructionSet, definition);
            Inspectors.Add(element.Name, nodeInspector);
        }
    }

    private void OnSelectedGraphElement(IGraphElement element)
    {
        if (element is GraphJoint)
            return;

        // Retrieve element's inspector.
        Control inspector = Inspectors[((Node)element).Name];

        // Add element's inspector to inspector window.
        InspectorWindow.Add(inspector);
    }

    private void OnDeselectedGraphElement(IGraphElement element)
    {
        if (element is GraphJoint)
            return;

        // Retrieve element's inspector.
        Control inspector = Inspectors[((Node)element).Name];

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
        Inspectors.Remove(((Node)element).Name);
    }

    private static T GetParameter<T>(InstructionDefinition definition, string id) where T : Parameter
    {
        return definition.GetParameter(id) as T;
    }
}
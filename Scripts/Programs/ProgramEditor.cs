using Godot;

namespace Rusty.ISA.Editor;

[GlobalClass]
public partial class ProgramEditor : MarginContainer
{
    /* Public properties. */

    /* Private properties. */
    private InstructionSet InstructionSet { get; set; }

    private GraphEdit GraphEdit { get; set; }
    private ContextMenu ContextMenu { get; set; }

    /* Constructors. */
    public ProgramEditor(InstructionSet set)
    {
        InstructionSet = set;

        MouseFilter = MouseFilterEnum.Pass;

        // Add root container.
        HBoxContainer hbox = new();
        AddChild(hbox);

        // Create inspector window.
        InspectorWindow inspector = new();
        inspector.SizeFlagsHorizontal = SizeFlags.Fill;
        hbox.AddChild(inspector);
        inspector.Name = "Inspector";

        // Create resizer.
        Resizer resizer = new();
        resizer.Target = inspector;
        hbox.AddChild(resizer);
        resizer.Name = "Resizer";

        // Create graph.
        GraphEdit = new();
        GraphEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
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
        switch (definition.Opcode)
        {
            case BuiltIn.JointOpcode:
                GraphJoint joint = GraphEdit.AddJoint(spawnX, spawnY);
                joint.BgColor = definition.EditorNode.MainColor;
                break;
            case BuiltIn.CommentOpcode:
                GraphComment comment = GraphEdit.AddComment(spawnX, spawnY);
                comment.CommentText = definition.GetParameter<MultilineParameter>(BuiltIn.CommentText).DefaultValue;
                comment.BgColor = definition.EditorNode.MainColor;
                comment.TextColor = definition.EditorNode.TextColor;
                break;
            case BuiltIn.FrameOpcode:
                GraphFrame frame = GraphEdit.AddFrame(spawnX, spawnY);
                frame.Title = definition.GetParameter<TextlineParameter>(BuiltIn.FrameTitle).DefaultValue;
                frame.TintColor = definition.GetParameter<ColorParameter>(BuiltIn.FrameColor).DefaultValue;
                break;
            default:
                GraphNode node = GraphEdit.AddNode(spawnX, spawnY);
                node.TitleText = definition.DisplayName;
                node.TitleIcon = definition.Icon;
                node.TitleColor = definition.EditorNode.MainColor;
                break;
        }
    }
}
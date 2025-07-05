using Godot;

namespace Rusty.ISA.Editor;

[GlobalClass]
public partial class ProgramEditor : MarginContainer
{
    /* Public properties. */

    /* Private properties. */
    private InstructionSet InstructionSet { get; set; }

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
        GraphEdit graphEdit = new();
        graphEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        graphEdit.RightClicked += OnRightClickedGraph;
        hbox.AddChild(graphEdit);
        graphEdit.Name = "GraphEdit";

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
        GD.Print("Must spawn " + definition);
    }
}
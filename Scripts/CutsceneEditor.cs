using Godot;

namespace Rusty.ISA.Editor;

[GlobalClass]
public partial class CutsceneEditor : MarginContainer
{
    [Export] InstructionSet BuiltIn { get; set; }
    [Export] string FolderPath { get; set; } = "res://.godot/Definitions";
    [Export] new string Name { get; set; } = "InstructionSet";
    [Export(PropertyHint.MultilineText)] string Description { get; set; } = "";
    [Export] string Author { get; set; } = "";
    [Export] string Version { get; set; } = "1.0.0";

    /* Godot overrides. */
    public override void _Ready()
    {
        InstructionSet set = InstructionSetBuilder.Build(Name, Description, Author, Version, BuiltIn, FolderPath);

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
        hbox.AddChild(graphEdit);
        graphEdit.Name = "GraphEdit";

        // Add debug elements.
        graphEdit.AddNode(200, 200);
        graphEdit.AddJoint(400, 200);
        graphEdit.AddComment(600, 200);
        graphEdit.AddFrame(800, 200);
    }
}
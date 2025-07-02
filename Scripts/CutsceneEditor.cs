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

        GraphEdit graphEdit = new();
        AddChild(graphEdit);

        GraphNode node = new();
        node.CustomMinimumSize = new Vector2(100, 100);
        graphEdit.AddElement(node);

        GraphComment comment = new();
        comment.CustomMinimumSize = new Vector2(100, 100);
        graphEdit.AddElement(comment);

        GraphFrame frame = new();
        frame.CustomMinimumSize = new Vector2(100, 100);
        graphEdit.AddElement(frame);
    }
}
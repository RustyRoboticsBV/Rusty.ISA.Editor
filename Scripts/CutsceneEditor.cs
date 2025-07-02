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
        var set = InstructionSetBuilder.Build(Name, Description, Author, Version, BuiltIn, FolderPath);
        GD.Print(set);

        string path = PathUtility.GetPath($"{FolderPath}/../{Name}.zip");
        GD.Print($"Written set {set.Name} to " + path);
        System.IO.File.WriteAllBytes(path, SetSerializer.Serialize(set));
    }
}
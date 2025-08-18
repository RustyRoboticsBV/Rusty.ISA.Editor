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
    [Export] Font ConsoleFont { get; set; }

    /* Godot overrides. */
    public override void _Ready()
    {
        // Build instruction set.
        InstructionSet set = InstructionSetBuilder.Build(Name, Description, Author, Version, BuiltIn, FolderPath);

        // Create program editor.
        ProgramEditor programEditor = new(set);

        // Create console.
        Console console = new();
        console.Font = ConsoleFont;
        Log.Console = console;
        console.SizeFlagsHorizontal = SizeFlags.ExpandFill;

        ScrollContainer consoleScroller = new();
        consoleScroller.AddChild(console);

        // Create vbox.
        VSplitContainer vbox = new(programEditor, consoleScroller);
        vbox.TopMinSize = 8;
        vbox.BottomMinSize = 8f;
        vbox.CurrentFactor = 0.8f;
        AddChild(vbox);
    }
}
using Godot;
using System;

namespace Rusty.ActionGraph;

using Rusty.ActionGraph.Consoles;

[GlobalClass]
public partial class IsaEditor : VBoxContainer
{
    [Export] Font ConsoleFont { get; set; }

    public override void _EnterTree()
    {
        Console console = new();
        console.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        console.SizeFlagsVertical = SizeFlags.ExpandFill;
        console.Font = ConsoleFont;
        AddChild(console);
        Log.Console = console;
    }
}

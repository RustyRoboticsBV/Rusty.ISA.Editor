using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A base class for graph editor window.
/// </summary>
public abstract partial class EditorWindow : VBoxContainer
{
    /* Public properties. */
    [Export] public Font ConsoleFont { get; set; }

    /* Internal properties. */
    internal Inspector Inspector { get; private set; }
    internal Graph Graph { get; private set; }
    internal Console Console { get; private set; }

    /* Constructors. */
    public EditorWindow()
    {
        HBoxContainer hbox = new();
        hbox.Name = "Inspector+Graph";
        hbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        hbox.SizeFlagsVertical = SizeFlags.ExpandFill;
        AddChild(hbox);

        Inspector = new();
        Inspector.Name = "Inspector";
        Inspector.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Inspector.SizeFlagsVertical = SizeFlags.ExpandFill;
        hbox.AddChild(Inspector);

        Graph = new();
        Graph.Name = "Graph";
        Graph.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Graph.SizeFlagsVertical = SizeFlags.ExpandFill;
        hbox.AddChild(Graph);

        Console = new();
        Console.Name = "Console";
        Console.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Console.SizeFlagsVertical = SizeFlags.ExpandFill;
        Console.Font = ConsoleFont;
        AddChild(Console);
    }
}

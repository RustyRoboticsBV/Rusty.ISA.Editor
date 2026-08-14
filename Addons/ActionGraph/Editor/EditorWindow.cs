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

    /* Private properties. */
    private UndoRedo UndoRedo { get; set; }
    private bool UndoRedoAllowed { get; set; } = true;

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

        // TODO: temporary undo/redo testing. Remove later.
        void SetupUndoRedo(UndoRedo undoRedo, Node node)
        {
            if (node is CheckBox cb)
                cb.UndoRedo = undoRedo;
            else if (node is CheckButton cb2)
                cb2.UndoRedo = undoRedo;
            else if (node is LineEdit le)
                le.UndoRedo = undoRedo;
            else if (node is TextEdit te)
                te.UndoRedo = undoRedo;
            else if (node is ColorPickerButton cpb)
                cpb.UndoRedo = undoRedo;
            else if (node is SpinBox sb)
                sb.UndoRedo = undoRedo;
            else if (node is HSlider hs)
                hs.UndoRedo = undoRedo;
            else if (node is OptionButton ob)
                ob.UndoRedo = undoRedo;
            else if (node is IWidget widget)
                widget.UndoRedo = UndoRedo;
            foreach (Node child in node.GetChildren())
            {
                SetupUndoRedo(undoRedo, child);
            }
        }
        UndoRedo = new();
        SetupUndoRedo(UndoRedo, this);
    }

    /* Public methods. */
    public override void _Process(double delta)
    {
        if (KeyboardShortcuts.PressedUndo)
            UndoRedo.Undo();
        else if (KeyboardShortcuts.PressedRedo)
            UndoRedo.Redo();
    }
}

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

        Control graphContainer = new();
        graphContainer.Name = "GraphContainer";
        graphContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        graphContainer.SizeFlagsVertical = SizeFlags.ExpandFill;
        hbox.AddChild(graphContainer);

        Graph = new();
        Graph.Name = "Graph";
        Graph.AnchorBottom = 1;
        Graph.AnchorRight = 1;
        graphContainer.AddChild(Graph);

        MouseLabel mouseLabel = new();
        mouseLabel.Name = "MouseLabel";
        mouseLabel.Graph = Graph;
        mouseLabel.AnchorRight = 1;
        mouseLabel.AnchorTop = 1;
        mouseLabel.AnchorBottom = 1;
        mouseLabel.Position = new(8, -36);
        graphContainer.AddChild(mouseLabel);

        Console = new();
        Console.Name = "Console";
        Console.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Console.SizeFlagsVertical = SizeFlags.ExpandFill;
        Console.Font = ConsoleFont;
        AddChild(Console);

        // TODO: temporary undo/redo testing. Remove later.
        void SetupUndoRedo(UndoRedo undoRedo, Node node)
        {
            if (node is IWidget widget)
            {
                widget.UndoRedo = UndoRedo;
                return;
            }
            foreach (Node child in node.GetChildren())
            {
                SetupUndoRedo(undoRedo, child);
            }
        }
        UndoRedo = new();
        SetupUndoRedo(UndoRedo, this);
    }

    /* Public methods. */
    /// <summary>
    /// Add a new node type to the editor.
    /// </summary>
    public void AddNodeDefinition(NodeDefinition node) => Graph.RegisterDefinition(node);

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        if (KeyboardShortcuts.PressedUndo)
            UndoRedo.Undo();
        else if (KeyboardShortcuts.PressedRedo)
            UndoRedo.Redo();
    }
}
using Godot;
using System;

namespace Rusty.ISA.Editor;

[GlobalClass]
public partial class ProgramEditor : MarginContainer
{
    /* Private properties. */
    private InstructionSet InstructionSet { get; set; }

    private Button CopyButton { get; set; }
    private Button PasteButton { get; set; }
    private InspectorWindow InspectorWindow { get; set; }
    private GraphEdit GraphEdit { get; set; }
    private ContextMenu ContextMenu { get; set; }

    private Ledger Ledger { get; set; }
    private int NextFrameID { get; set; } = 0;

    private bool CompressCode { get; set; } = false;

    /* Constructors. */
    public ProgramEditor(InstructionSet set)
    {
        InstructionSet = set;

        MouseFilter = MouseFilterEnum.Pass;

        // Add background.
        ColorRect background = new();
        background.Color = new(0.5f, 0.5f, 0.5f);
        AddChild(background);

        // Add vertical box.
        VBoxContainer vbox = new();
        AddChild(vbox);

        // Add buttons.
        HBoxContainer buttons = new();
        vbox.AddChild(buttons);

        CopyButton = new();
        CopyButton.Text = "Copy";
        CopyButton.Pressed += OnPressedCopy;
        buttons.AddChild(CopyButton);

        PasteButton = new();
        PasteButton.Text = "Paste";
        PasteButton.Pressed += OnPressedPaste;
        buttons.AddChild(PasteButton);

        // Create inspector window.
        InspectorWindow = new();
        InspectorWindow.SizeFlagsHorizontal = SizeFlags.Fill;
        InspectorWindow.Name = "Inspector";

        // Create graph.
        GraphEdit = new();
        GraphEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        GraphEdit.CustomMinimumSize = new(256f, 256f);
        GraphEdit.RightClicked += OnRightClickedGraph;
        GraphEdit.Name = "GraphEdit";

        // Create hbox.
        HSplitContainer hbox = new(InspectorWindow, GraphEdit);
        hbox.SizeFlagsVertical = SizeFlags.ExpandFill;
        hbox.LeftMinSize = 256f;
        hbox.RightMinSize = 256;
        hbox.CurrentFactor = 0.275f;
        vbox.AddChild(hbox);

        // Create context menu.
        ContextMenu = new();
        ContextMenu.UpdateInstructionSet(InstructionSet);
        ContextMenu.SelectedItem += OnMustSpawn;
        AddChild(ContextMenu);

        // Create program units container.
        Ledger = new(set, GraphEdit, InspectorWindow);
    }

    /* Private methods. */
    private void OnPressedCopy()
    {
        try
        {
            // Create syntax tree.
            SyntaxTree syntaxTree = new(Ledger);
            Log.Message("Compilation syntax tree:", syntaxTree);

            // Serialize to code.
            string code = syntaxTree.Compile();

            // Compress code if enabled.
            if (CompressCode)
                code = StringCompressor.Compress(code);

            // Write to clipboard.
            DisplayServer.ClipboardSet(code);
        }
        catch (Exception exception)
        {
            Log.Error($"An exception occurred during compilation. This should not happen. Please open an issue on the git repository, and include the text below:\n\n{exception}");
            if (OS.HasFeature("editor"))
                throw;
        }
    }

    private void OnPressedPaste()
    {
        // Read from clipboard.
        string code = DisplayServer.ClipboardGet();

        // Decompress if necessary.
        if (StringCompressor.IsCompressed(code))
            code = StringCompressor.Decompress(code);

        // Create syntax tree.
        SyntaxTree syntaxTree = new(InstructionSet, code);

        // Clear graph.
        Ledger.Clear();

        // Decompile syntax tree.
        Log.Message("Decompilation syntax tree:", syntaxTree);
        syntaxTree.Decompile(Ledger);
    }


    private void OnRightClickedGraph()
    {
        ContextMenu.Position = (Vector2I)GetGlobalMousePosition();
        ContextMenu.Show();
    }

    private void OnMustSpawn(InstructionDefinition definition)
    {
        // Get spawn position.
        Vector2 globalSpawnPosition = ContextMenu.Position;
        Vector2 spawnPosition = GraphEdit.GetPositionOffsetFromGlobalPosition(globalSpawnPosition);
        int spawnX = (int)spawnPosition.X;
        int spawnY = (int)spawnPosition.Y;
        if (GraphEdit.SnappingEnabled)
        {
            float snap = GraphEdit.SnappingDistance;
            spawnX = (int)(Mathf.Floor(spawnX / snap) * snap);
            spawnY = (int)(Mathf.Floor(spawnY / snap) * snap);
        }

        // Spawn.
        Ledger.CreateElement(definition, new(spawnX, spawnY));
    }
}
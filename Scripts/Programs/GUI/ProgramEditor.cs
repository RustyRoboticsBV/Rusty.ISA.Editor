using Godot;
using System;

namespace Rusty.ISA.Editor;

[GlobalClass]
public partial class ProgramEditor : MarginContainer
{
    /* Private properties. */
    private InstructionSet InstructionSet { get; set; }

    private PopupButton File { get; set; }
    private PopupButton Edit { get; set; }
    private Button CopyButton { get; set; }
    private Button PasteButton { get; set; }
    private LanguageDropdown Languages { get; set; }

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

        // Add margins.
        AddThemeConstantOverride("margin_left", 4);
        AddThemeConstantOverride("margin_top", 4);

        // Add background.
        ColorRect background = new();
        background.Color = EditorColors.Background;
        AddChild(background);

        // Add vertical box.
        VBoxContainer vbox = new();
        AddChild(vbox);

        // Add buttons.
        HBoxContainer buttons = new();
        vbox.AddChild(buttons);

        File = new();
        File.ButtonText = "File";
        File.Options = ["New", "Save As", "Open", "Copy All", "Paste All"];
        File.CustomMinimumSize = new(96, 0);
        File.PressedOption += OnPressedFileOption;
        buttons.AddChild(File);

        Edit = new();
        Edit.ButtonText = "Edit";
        Edit.Options = ["Undo", "Redo", "Copy", "Paste", "Delete", "Create Node"];
        Edit.CustomMinimumSize = new(96, 0);
        Edit.PressedOption += OnPressedEditOption;
        buttons.AddChild(Edit);

        VSeparator vseparator = new();
        buttons.AddChild(vseparator);

        Languages = new();
        buttons.AddChild(Languages);

        // Create inspector dock.
        VBoxContainer leftDock = new();

        // Create inspector window.
        InspectorWindow = new();
        InspectorWindow.SizeFlagsHorizontal = SizeFlags.Fill;
        InspectorWindow.SizeFlagsVertical = SizeFlags.ExpandFill;
        InspectorWindow.Name = "Inspector";
        leftDock.AddChild(InspectorWindow);

        // Create graph.
        GraphEdit = new();
        GraphEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        GraphEdit.CustomMinimumSize = new(256f, 256f);
        GraphEdit.RightClicked += OnRightClickedGraph;
        GraphEdit.Name = "GraphEdit";

        // Create hbox.
        HSplitContainer hbox = new(leftDock, GraphEdit);
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
        Ledger = new(set, GraphEdit, InspectorWindow, Languages);
    }

    /* Private methods. */
    private void OnPressedFileOption(long index)
    {
        switch (index)
        {
            case 0:
                OnPressedNew();
                break;
            case 1:
                OnPressedSave();
                break;
            case 2:
                OnPressedOpen();
                break;
            case 3:
                OnPressedCopyAll();
                break;
            case 4:
                OnPressedPasteAll();
                break;
        }
    }

    private void OnPressedNew()
    {
        Log.Error("New as is not yet implemented.");
    }

    private void OnPressedSave()
    {
        Log.Error("Save as is not yet implemented.");
    }

    private void OnPressedOpen()
    {
        Log.Error("Open file is not yet implemented.");
    }

    private void OnPressedCopyAll()
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

    private void OnPressedPasteAll()
    {
        // Read from clipboard.
        string code = DisplayServer.ClipboardGet();

        // Decompress if necessary.
        if (StringCompressor.IsCompressed(code))
            code = StringCompressor.Decompress(code);

        // Create syntax tree.
        SyntaxTree syntaxTree = new(InstructionSet, code);

        // Clear graph.
        Ledger.ClearGraph();

        // Decompile syntax tree.
        Log.Message("Decompilation syntax tree:", syntaxTree);
        syntaxTree.Decompile(Ledger);
    }


    private void OnPressedEditOption(long index)
    {
        switch (index)
        {
            case 0:
                OnPressedUndo();
                break;
            case 1:
                OnPressedRedo();
                break;
            case 2:
                OnPressedCopy();
                break;
            case 3:
                OnPressedPaste();
                break;
            case 4:
                OnPressedDelete();
                break;
            case 5:
                OnPressedCreateNode();
                break;
        }
    }

    private void OnPressedUndo()
    {
        Log.Error("Undo not yet implemented.");
    }

    private void OnPressedRedo()
    {
        Log.Error("Redo not yet implemented.");
    }

    private void OnPressedCopy()
    {
        Log.Error("Copy not yet implemented.");
    }

    private void OnPressedPaste()
    {
        Log.Error("Paste not yet implemented.");
    }

    private void OnPressedDelete()
    {
        Log.Error("Delete not yet implemented.");
    }

    private void OnPressedCreateNode()
    {
        ContextMenu.Position = (Vector2I)(GraphEdit.GlobalPosition + GraphEdit.Size / 10f);
        ContextMenu.Show();
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
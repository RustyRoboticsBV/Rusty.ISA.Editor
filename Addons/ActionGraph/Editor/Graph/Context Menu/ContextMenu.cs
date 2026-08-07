using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// The context menu used for creating new graph elements.
/// </summary>
internal partial class ContextMenu : PopupMenu
{
    /* Private methods. */
    private PopupMenu NodesMenu { get; set; }

    /* Public events. */
    public event Action<int> NodeSelected;
    public event Action MemoSelected;
    public event Action FrameSelected;

    /* Constructors. */
    public ContextMenu()
    {
        // Find directory path.
        string scriptDir = ((CSharpScript)GetScript()).ResourcePath.GetBaseDir();

        // Add built-in items.
        AddSeparator("Add Element");

        NodesMenu = new();
        NodesMenu.AddSeparator("Nodes");
        NodesMenu.IdPressed += OnNodeSelected;
        AddSubmenuNodeItem("Nodes", NodesMenu);

        Texture2D textureFrame = GD.Load<Texture2D>($"{scriptDir}/Icons/Frame.svg");
        AddIconItem(textureFrame, "Frame");

        Texture2D textureMemo = GD.Load<Texture2D>($"{scriptDir}/Icons/Memo.svg");
        AddIconItem(textureMemo, "Sticky Note");

        IdPressed += OnItemSelected;
    }

    /* Public methods. */
    /// <summary>
    /// Add a node item to the context menu.
    /// </summary>
    public void AddNode(Texture2D icon, string label)
    {
        NodesMenu.AddIconItem(icon, label);
    }

    /* Private methods. */
    private void OnItemSelected(long id)
    {
        if (id == 2)
            FrameSelected?.Invoke();
        else if (id == 3)
            MemoSelected?.Invoke();
    }

    private void OnNodeSelected(long id)
    {
        NodeSelected?.Invoke(NodesMenu.GetItemIndex((int)id) - 1);
    }
}
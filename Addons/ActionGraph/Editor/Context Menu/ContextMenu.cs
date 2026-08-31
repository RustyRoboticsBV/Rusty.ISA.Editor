using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// The context menu used for creating new graph elements.
/// </summary>
internal sealed partial class ContextMenu : PopupMenu
{
    /* Private methods. */
    private PopupMenu NodesMenu { get; set; }
    private Dictionary<string, NodeSubMenu> Categories { get; } = new();

    /* Public events. */
    public event Action<NodeDefinition> NodeSelected;
    public event Action MemoSelected;
    public event Action FrameSelected;

    /* Constructors. */
    public ContextMenu()
    {
        // Find directory path.
        string scriptDir = ((CSharpScript)GetScript()).ResourcePath.GetBaseDir();

        // Add built-in items.
        AddSeparator("Add Element");

        Texture2D textureMemo = GD.Load<Texture2D>($"{scriptDir}/Icons/Memo.svg");
        AddIconItem(textureMemo, "Sticky Note");

        Texture2D textureFrame = GD.Load<Texture2D>($"{scriptDir}/Icons/Frame.svg");
        AddIconItem(textureFrame, "Frame");

        IdPressed += (long id) =>
        {
            if (id == 1)
                MemoSelected?.Invoke();
            else if (id == 2)
                FrameSelected?.Invoke();
        };
    }

    /* Public methods. */
    public void AddNode(NodeDefinition definition)
    {
        // Add category.
        if (!Categories.ContainsKey(definition.Category))
        {
            NodeSubMenu submenu = new(definition.Category);
            submenu.NodeSelected += (node) => NodeSelected?.Invoke(node);
            Categories[definition.Category] = submenu;
            AddSubmenuNodeItem(definition.Category, submenu);
        }

        // Add item.
        Categories[definition.Category].AddNode(definition);
    }
}
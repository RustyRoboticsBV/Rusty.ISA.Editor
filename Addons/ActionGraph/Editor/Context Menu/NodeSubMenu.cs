using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A submenu of the context menu that represents a single node category.
/// </summary>
internal sealed partial class NodeSubMenu : PopupMenu
{
    /* Private methods. */
    private List<NodeDefinition> NodeDefinitions { get; } = new();

    /* Public events. */
    public event Action<NodeDefinition> NodeSelected;

    /* Constructors. */
    public NodeSubMenu(string category)
    {
        AddSeparator(category);
        IdPressed += (index) => NodeSelected?.Invoke(NodeDefinitions[(int)index - 1]);
    }

    /* Public methods. */
    public void AddNode(NodeDefinition definition)
    {
        NodeDefinitions.Add(definition);
        AddIconItem(definition.Icon, definition.Title);
    }
}
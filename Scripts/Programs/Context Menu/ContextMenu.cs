using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// The "add node" popup menu used by the graph editor.
/// </summary>
public partial class ContextMenu : PopupMenu
{
    /* Public properties. */
    public string MenuName { get; set; } = "Add Node";
    public List<InstructionDefinition> Definitions { get; private set; } = new();
    public List<PopupMenu> Submenus { get; set; } = new();

    /* Public delegates. */
    public delegate void SelectionHandler(InstructionDefinition definition);

    /* Public events. */
    /// <summary>
    /// Gets invoked whenever an instruction definition is selected by the user.
    /// </summary>
    public event SelectionHandler SelectedItem;

    /* Constructors. */
    public ContextMenu()
    {
        // Set min width.
        MinSize = new(190, MinSize.Y);

        // Add title.
        AddSeparator(MenuName);
        SetItemDisabled(0, true);
        SetItemShortcutDisabled(0, true);

        // Set up events.
        IdPressed += OnSelectedItem;
    }

    /* Public methods. */
    /// <summary>
    /// Update the items in the menu.
    /// </summary>
    public void UpdateInstructionSet(InstructionSet set)
    {
        Clear();

        if (set == null)
            return;

        // Find all node instructions, and group them by category and priority.
        Dictionary<string, List<InstructionDefinition>> nodeInstructions = new();

        for (int i = 0; i < set.Definitions.Length; i++)
        {
            InstructionDefinition definition = set.Definitions[i];

            // Skip if not an editor node.
            if (definition.EditorNode == null)
                continue;

            // Add category list if it didn't exist yet.
            if (!nodeInstructions.ContainsKey(definition.Category))
                nodeInstructions.Add(definition.Category, new());

            // Get category list.
            List<InstructionDefinition> category = nodeInstructions[definition.Category];

            // Place according to priority.
            bool added = false;
            for (int j = category.Count - 1; j >= 0; j--)
            {
                if (definition.EditorNode.Priority >= category[j].EditorNode.Priority)
                {
                    category.Insert(j + 1, definition);
                    added = true;
                    break;
                }
            }
            if (!added)
                category.Insert(0, definition);
        }

        // Add nodes that don't have a category.
        if (nodeInstructions.ContainsKey(""))
        {
            foreach (InstructionDefinition definition in nodeInstructions[""])
            {
                AddDefinition(definition);
            }
        }

        // Create submenus for each category.
        foreach (string categoryName in nodeInstructions.Keys)
        {
            AddCategory(categoryName, nodeInstructions[categoryName]);
        }
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        SetItemText(0, MenuName);
    }

    /* Private methods. */
    private void AddDefinition(InstructionDefinition definition)
    {
        AddIconItem(definition.Icon, definition.DisplayName, Definitions.Count);
        SetItemTooltip(Definitions.Count + 1, definition.Description);
        Definitions.Add(definition);
    }

    private ContextMenu AddCategory(string name, List<InstructionDefinition> definitions)
    {
        // Create submenu.
        ContextMenu submenu = new();
        submenu.MenuName = name;
        Submenus.Add(submenu);
        AddSubmenuNodeItem(name, submenu);

        // Set up events.
        submenu.SelectedItem += OnSelectedSubmenuItem;

        // Add items.
        foreach (InstructionDefinition definition in definitions)
        {
            submenu.AddDefinition(definition);
        }

        return submenu;
    }

    private void OnSelectedItem(long index)
    {
        SelectedItem?.Invoke(Definitions[(int)index]);
    }

    private void OnSelectedSubmenuItem(InstructionDefinition definition)
    {
        SelectedItem?.Invoke(definition);
    }
}
using Godot;
using System;
using System.Collections.Generic;
using Rusty.Cutscenes;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// The add node popup menu used by the graph editor.
    /// </summary>
    [GlobalClass]
    public partial class AddNodePopup : PopupMenu
    {
        /* Public properties. */
        public string MenuName { get; set; } = "";
        public List<InstructionDefinition> Definitions { get; set; } = new();
        public List<PopupMenu> Submenus { get; set; } = new();

        /* Delegates. */
        public delegate void SelectInstructionDefinitionHandler(InstructionDefinition definition);

        /* Events. */
        public event SelectInstructionDefinitionHandler SelectedInstruction;

        /* Constructors. */
        public AddNodePopup() : this("Add Node") { }

        public AddNodePopup(string name)
        {
            // Set min width.
            MinSize = new(190, MinSize.Y);

            // Add title.
            MenuName = name;
            AddSeparator(MenuName);

            // Set up events.
            IdPressed += OnIdPressed;
        }

        public AddNodePopup(InstructionDefinition[] definitions) : this()
        {
            if (definitions == null)
                return;

            // Find all node instructions, and group them by category and priority.
            Dictionary<string, List<InstructionDefinition>> nodeInstructions = new();

            for (int i = 0; i < definitions.Length; i++)
            {
                InstructionDefinition definition = definitions[i];

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

        public AddNodePopup(InstructionSet instructionSet) : this(instructionSet != null ? instructionSet.Definitions : null) { }

        /* Private methods. */
        private void AddDefinition(InstructionDefinition definition)
        {
            AddIconItem(definition.Icon, definition.DisplayName, Definitions.Count);
            SetItemTooltip(Definitions.Count + 1, definition.Description);
            Definitions.Add(definition);
        }

        private AddNodePopup AddCategory(string name, List<InstructionDefinition> definitions)
        {
            // Create submenu.
            AddNodePopup submenu = new(name);
            submenu.MenuName = name;
            Submenus.Add(submenu);
            AddSubmenuNodeItem(name, submenu);

            // Set up events.
            submenu.SelectedInstruction += OnSelectedSubmenuInstruction;

            // Add items.
            foreach (InstructionDefinition definition in definitions)
            {
                submenu.AddDefinition(definition);
            }

            return submenu;
        }

        private void OnIdPressed(long index)
        {
            SelectedInstruction?.Invoke(Definitions[(int)index]);
        }

        private void OnSelectedSubmenuInstruction(InstructionDefinition definition)
        {
            SelectedInstruction?.Invoke(definition);
        }
    }
}
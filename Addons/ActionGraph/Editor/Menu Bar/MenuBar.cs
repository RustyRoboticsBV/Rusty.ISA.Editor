using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// The editor menu bar.
/// </summary>
internal sealed partial class MenuBar : HBoxContainer
{
    /* Public events. */
    public event Action PressedNew;
    public event Action PressedSave;
    public event Action PressedSaveAs;
    public event Action PressedOpen;

    public event Action PressedUndo;
    public event Action PressedRedo;
    public event Action PressedCopy;
    public event Action PressedPaste;
    public event Action PressedDelete;

    /* Constructors. */
    public MenuBar()
    {
        MenuBarButton fileButton = new();
        fileButton.Text = "File";
        fileButton.AddItem("New");
        fileButton.AddItem("Save");
        fileButton.AddItem("Save As");
        fileButton.AddItem("Open");
        fileButton.SelectedText += (index) =>
        {
            if (index == 0)
                PressedNew?.Invoke();
            else if (index == 1)
                PressedSave?.Invoke();
            else if (index == 2)
                PressedSaveAs?.Invoke();
            else if (index == 3)
                PressedOpen?.Invoke();
        };
        AddChild(fileButton);

        MenuBarButton editButton = new();
        editButton.Text = "Edit";
        editButton.AddItem("Undo");
        editButton.AddItem("Redo");
        editButton.AddSeparator();
        editButton.AddItem("Copy");
        editButton.AddItem("Paste");
        editButton.AddSeparator();
        editButton.AddItem("Delete");
        editButton.SelectedText += (index) =>
        {
            if (index == 0)
                PressedUndo?.Invoke();
            else if (index == 1)
                PressedRedo?.Invoke();
            else if (index == 3)
                PressedCopy?.Invoke();
            else if (index == 4)
                PressedPaste?.Invoke();
            else if (index == 6)
                PressedDelete?.Invoke();
        };
        AddChild(editButton);
    }
}
using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A single button of the menu bar.
/// </summary>
internal sealed partial class MenuBarButton : MenuButton
{
    /* Private methods. */
    private PopupMenu Popup { get; set; }

    /* Public events. */
    public event Action<int> SelectedText;

    /* Constructors. */
    public MenuBarButton()
    {
        CustomMinimumSize = new(64, 0);
        Flat = false;
        Popup = GetPopup();
        Popup.IndexPressed += (index) => SelectedText?.Invoke((int)index);
    }

    /* Public methods. */
    /// <summary>
    /// Add an item to the menu.
    /// </summary>
    public void AddItem(string text) => Popup.AddItem(text);

    /// <summary>
    /// Add a separator to the menu.
    /// </summary>
    public void AddSeparator() => Popup.AddSeparator();
}
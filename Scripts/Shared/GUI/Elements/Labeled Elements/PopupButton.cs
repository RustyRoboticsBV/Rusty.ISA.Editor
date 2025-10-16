using Godot;
using System;

namespace Rusty.ISA.Editor;

/// <summary>
/// A popup button element.
/// </summary>
public sealed partial class PopupButton : LabeledElement
{
    /* Public properties. */
    /// <summary>
    /// The text that is displayed on the button.
    /// </summary>
    public string ButtonText
    {
        get => MenuButton.Text;
        set => MenuButton.Text = value;
    }
    /// <summary>
    /// The options in the popup menu.
    /// </summary>
    public string[] Options
    {
        get
        {
            if (MenuButton == null)
                return [];
            PopupMenu popup = MenuButton.GetPopup();
            int itemCount = popup.ItemCount;
            string[] options = new string[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                options[i] = popup.GetItemText(i);
            }
            return options;
        }
        set
        {
            if (MenuButton == null)
                return;
            PopupMenu popup = MenuButton.GetPopup();
            popup.Clear();
            foreach (string option in value)
            {
                popup.AddItem(option);
            }
        }
    }

    /* Private properties. */
    private Button FakeButton { get; set; }
    private MenuButton MenuButton { get; set; }

    /* Public events. */
    public event Action<long> PressedOption;

    /* Constructors. */
    public PopupButton() : base()
    {
        // Add margin.
        MarginContainer container = new();
        container.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(container);
        container.Name = "MarginContainer";

        // Add fake button for a background.
        FakeButton = new();
        container.AddChild(FakeButton);
        FakeButton.Name = "Background";

        // Add real button.
        MenuButton = new();
        container.AddChild(MenuButton);
        MenuButton.Name = "MenuButton";

        // Set up events.
        MenuButton.GetPopup().IdPressed += OnPopupPressed;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        PopupButton copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public override void CopyFrom(IGuiElement other)
    {
        base.CopyFrom(other);

        if (other is PopupButton popup)
        {
            ButtonText = popup.ButtonText;
            Options = popup.Options;
        }
    }

    /* Private methods. */
    private void OnPopupPressed(long id)
    {
        PressedOption?.Invoke(id);
    }
}
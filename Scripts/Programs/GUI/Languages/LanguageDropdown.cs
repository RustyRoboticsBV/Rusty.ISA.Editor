using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A collection of UI elements for selecting, adding, removing, renaming and reordering languages.
/// </summary>
public partial class LanguageDropdown : HBoxContainer
{
    /* Constants. */
    private const string IconPath = "res://Submodules/ISA/Built-In Instructions/IconLAN.svg";

    /* Private properties. */
    private OptionButton Dropdown { get; set; }
    private LineEditPopup AddPopup { get; set; }
    private ConfirmationDialog RemovePopup { get; set; }
    private LineEditPopup RenamePopup { get; set; }
    private Button RemoveButton { get; set; }
    private Button RenameButton { get; set; }
    private Button UpButton { get; set; }
    private Button DownButton { get; set; }

    /* Constructors. */
    public LanguageDropdown()
    {
        // Create icon.
        TextureRect icon = new();
        icon.Texture = ResourceLoader.Load<Texture2D>(IconPath);
        icon.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
        icon.StretchMode = TextureRect.StretchModeEnum.Scale;
        icon.TextureFilter = TextureFilterEnum.LinearWithMipmaps;
        icon.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
        icon.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        icon.CustomMinimumSize = Vector2.One * 32;
        AddChild(icon);

        // Create selector dropdown.
        Dropdown = new();
        Dropdown.CustomMinimumSize = new(128, 0);
        Dropdown.TooltipText = "Select a language. This allows you to view and set\nalternate values for any node parameter "
            + "that can\nbe localized to different languages.";
        Dropdown.AddItem("(default)");
        AddChild(Dropdown);

        // Create +, -, rename and arrow buttons.
        Button addButton = new();
        addButton.Text = "+";
        addButton.CustomMinimumSize = new(32, 0);
        addButton.TooltipText = "Add a new language.";
        addButton.Pressed += OnAddButtonPressed;
        AddChild(addButton);

        RemoveButton = new();
        RemoveButton.Text = "-";
        RemoveButton.CustomMinimumSize = new(32, 0);
        RemoveButton.TooltipText = "Remove the currently-selected language.";
        RemoveButton.Pressed += OnRemoveButtonPressed;
        AddChild(RemoveButton);

        RenameButton = new();
        RenameButton.Text = "\u270E";
        RenameButton.CustomMinimumSize = new(32, 0);
        RenameButton.TooltipText = "Rename the currently-selected language.";
        RenameButton.Pressed += OnRenameButtonPressed;
        AddChild(RenameButton);

        Control swapContainer = new();
        swapContainer.CustomMinimumSize = new(32, 32);
        AddChild(swapContainer);

        UpButton = new();
        UpButton.Text = "\u25B2";
        UpButton.CustomMinimumSize = new(32, 0);
        UpButton.Scale = new(1f, 0.5f);
        UpButton.TooltipText = "Move this language up in the list by one spot.";
        UpButton.Pressed += OnUpButtonPressed;
        swapContainer.AddChild(UpButton);

        DownButton = new();
        DownButton.Text = "\u25BC";
        DownButton.CustomMinimumSize = new(32, 0);
        DownButton.Position = new(0f, 16f);
        DownButton.Scale = new(1f, 0.5f);
        DownButton.TooltipText = "Move this language down in the list by one spot.";
        DownButton.Pressed += OnDownButtonPressed;
        swapContainer.AddChild(DownButton);

        // Create add language popup window.
        AddPopup = new();
        AddPopup.Title = "Add a new language...";
        AddPopup.Confirmed += OnAddConfirmed;
        AddPopup.Canceled += OnAddCancelled;
        AddChild(AddPopup);
        AddPopup.Hide();

        // Create remove language popup window.
        RemovePopup = new();
        RemovePopup.ExtendToTitle = true;
        RemovePopup.KeepTitleVisible = true;
        RemovePopup.Size = new(512, RemovePopup.Size.Y);
        RemovePopup.Confirmed += OnRemoveConfirmed;
        RemovePopup.Canceled += OnRemoveCancelled;
        AddChild(RemovePopup);
        RemovePopup.Hide();

        // Create rename language popup window.
        RenamePopup = new();
        RenamePopup.Confirmed += OnRenameConfirmed;
        RenamePopup.Canceled += OnRenameCancelled;
        AddChild(RenamePopup);
        RenamePopup.Hide();
    }

    /* Public methods. */
    /// <summary>
    /// Get the names of all user-defined languages.
    /// </summary>
    public string[] GetLanguages()
    {
        string[] languages = new string[Dropdown.ItemCount - 1];
        for (int i = 0; i < Dropdown.ItemCount - 1; i++)
        {
            languages[i] = Dropdown.GetItemText(i + 1);
        }
        return languages;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        RemoveButton.Disabled = Dropdown.Selected == 0;
        RenameButton.Disabled = Dropdown.Selected == 0;
        UpButton.Disabled = Dropdown.Selected <= 1;
        DownButton.Disabled = Dropdown.Selected == Dropdown.ItemCount - 1;
    }

    /* Private methods. */
    private void OnAddButtonPressed()
    {
        AddPopup.Show();
        AddPopup.MoveToCenter();
    }

    private void OnRemoveButtonPressed()
    {
        if (Dropdown.Selected > 0)
        {
            RemovePopup.Title = $"Remove language '{Dropdown.GetItemText(Dropdown.Selected)}'?";
            RemovePopup.Show();
            RemovePopup.MoveToCenter();
        }
    }

    private void OnRenameButtonPressed()
    {
        if (Dropdown.Selected > 0)
        {
            RenamePopup.Title = $"Rename language '{Dropdown.GetItemText(Dropdown.Selected)}'...";
            RenamePopup.Show();
            RenamePopup.MoveToCenter();
        }
    }

    private void OnUpButtonPressed()
    {
        string temp = Dropdown.GetItemText(Dropdown.Selected);
        Dropdown.SetItemText(Dropdown.Selected, Dropdown.GetItemText(Dropdown.Selected - 1));
        Dropdown.SetItemText(Dropdown.Selected - 1, temp);
        Dropdown.Select(Dropdown.Selected - 1);
    }

    private void OnDownButtonPressed()
    {
        string temp = Dropdown.GetItemText(Dropdown.Selected);
        Dropdown.SetItemText(Dropdown.Selected, Dropdown.GetItemText(Dropdown.Selected + 1));
        Dropdown.SetItemText(Dropdown.Selected + 1, temp);
        Dropdown.Select(Dropdown.Selected + 1);
    }

    private void OnAddConfirmed(string language)
    {
        // Hide and clear the popup.
        AddPopup.Text = "";
        AddPopup.Hide();

        // Make sure that the new language has a legal name.
        if (language == "")
        {
            Log.Error($"Cannot use the empty string as the name of a language!");
            return;
        }

        for (int i = 0; i < Dropdown.ItemCount; i++)
        {
            if (Dropdown.GetItemText(i) == language)
            {
                Log.Error($"Cannot add language '{language}' because it already exists!");
                return;
            }
        }

        // Add the language.
        Dropdown.AddItem(language);
        Dropdown.Select(Dropdown.ItemCount - 1);
    }

    private void OnAddCancelled()
    {
        AddPopup.Text = "";
        AddPopup.Hide();
    }

    private void OnRemoveConfirmed()
    {
        // Remove the selected language.
        int newindex = Dropdown.Selected - 1;
        Dropdown.RemoveItem(Dropdown.Selected);
        Dropdown.Select(newindex);

        // Hide the popup.
        RemovePopup.Hide();
    }

    private void OnRemoveCancelled()
    {
        RemovePopup.Hide();
    }

    private void OnRenameConfirmed(string language)
    {
        string oldName = Dropdown.GetItemText(Dropdown.Selected);

        // Hide and clear the popup.
        RenamePopup.Text = "";
        RenamePopup.Hide();

        // Make sure that the new name is a legal name.
        if (language == "")
        {
            Log.Error($"Cannot use the empty string as the name of a language!");
            return;
        }

        for (int i = 0; i < Dropdown.ItemCount; i++)
        {
            if (Dropdown.Selected == i)
                continue;

            if (Dropdown.GetItemText(i) == language)
            {
                Log.Error($"Cannot rename language '{oldName}' to '{language}' because that language already exists!");
                return;
            }
        }

        // Rename the language.
        Dropdown.SetItemText(Dropdown.Selected, language);
    }

    private void OnRenameCancelled()
    {
        RenamePopup.Text = "";
        RenamePopup.Hide();
    }
}
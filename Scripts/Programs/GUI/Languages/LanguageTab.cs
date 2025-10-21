using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// The language tab.
/// </summary>
public sealed partial class LanguageTab : DockWindow<Control>
{
    /* Public properties. */
    public new int ContentsCount => Mathf.Max(base.ContentsCount - 1, 0);

    /* Private properties. */
    Button AddButton { get; set; }

    /* Constructors. */
    public LanguageTab() : base()
    {
        TitleText = "Languages";

        // Add button.
        AddButton = new();
        AddButton.Text = "Add Language";
        AddButton.Pressed += OnAddButtonPressed;
        Add(AddButton);
    }

    /* Public methods. */
    public new LanguageField GetAt(int index)
    {
        return base.GetAt(index) as LanguageField;
    }

    public void AddLanguage(string id)
    {
        LanguageField field = new();
        field.ID = id;
        field.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        field.Deleted += OnFieldDeleted;
        Add(field);

        Remove(AddButton);
        Add(AddButton);

        FixLabels();
    }

    public string[] GetAllLanguages()
    {
        string[] languages = new string[ContentsCount];
        for (int i = 0; i < ContentsCount; i++)
        {
            languages[i] = GetAt(i).ID;
        }
        return languages;
    }

    public void Clear()
    {
        while (ContentsCount > 0)
        {
            Remove(GetAt(0));
        }
    }

    /* Private methods. */
    private void FixLabels()
    {
        for (int i = 0; i < ContentsCount; i++)
        {
            GetAt(i).LabelText = $"#{i + 1}";
        }
    }

    private void OnAddButtonPressed()
    {
        AddLanguage("language" + (ContentsCount + 1));
    }

    private void OnFieldDeleted(LanguageField field)
    {
        Remove(field);
        GD.Print("Deleting " + field);

        FixLabels();
    }
}
using Godot;
using System;

namespace Rusty.ISA.Editor;

public partial class LanguageField : HBoxContainer
{
    /* Public properties. */
    public string LabelText
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    public string ID
    {
        get => IDField.Value;
        set => IDField.Value = value;
    }

    /* Private properties. */
    private Label Label { get; set; }
    private LineField IDField { get; set; }
    private ToggleTextField IsDialectOfField { get; set; }

    /* Public events. */
    public event Action<LanguageField> Deleted;

    /* Constructors. */
    public LanguageField()
    {
        Label = new();
        Label.Text = "#0";
        Label.CustomMinimumSize = new(32f, 0f);
        AddChild(Label);

        IDField = new();
        IDField.Value = "new_language";
        IDField.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(IDField);

        Button deleteButton = new();
        deleteButton.Text = "Del";
        AddChild(deleteButton);
        deleteButton.Pressed += OnDeletePressed;
    }

    /* Private methods. */
    private void OnDeletePressed()
    {
        Deleted?.Invoke(this);
    }
}
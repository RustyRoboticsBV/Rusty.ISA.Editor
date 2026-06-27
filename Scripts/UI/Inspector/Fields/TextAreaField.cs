using Godot;
using System;

namespace Rusty.ISA;

/// <summary>
/// A text area field.
/// </summary>
[GlobalClass]
public partial class TextAreaField : VBoxContainer, IWidget, IValued<string>
{
    /* Public properties. */
    public string Title
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    public int TitleWidth
    {
        get => (int)Label.CustomMinimumSize.X;
        set => Label.CustomMinimumSize = new(value, Label.CustomMinimumSize.Y);
    }
    public string Description
    {
        get => TooltipText;
        set
        {
            TooltipText = value;
            Label.TooltipText = value;
            TextEdit.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo { get; set; }

    public string Value
    {
        get => TextEdit.Text;
        set => TextEdit.Text = value;
    }

    /* Private methods. */
    private Label Label { get; set; }
    private TextEdit TextEdit { get; set; }

    /* Public events. */
    public event Action<IWidget> Changed;

    /* Constructors. */
    public TextAreaField()
    {
        Label = new();
        AddChild(Label);
        Label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        TitleWidth = 160;

        TextEdit = new();
        AddChild(TextEdit);
        TextEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        TextEdit.TextChanged += OnChanged;
        TextEdit.CustomMinimumSize = new(0f, 100f);
    }

    /* Public methods. */
    public IWidget Copy()
    {
        TextAreaField field = new();
        field.SizeFlagsHorizontal = SizeFlagsHorizontal;
        field.SizeFlagsVertical = SizeFlagsVertical;
        field.Title = Title;
        field.TitleWidth = TitleWidth;
        field.Description = Description;
        field.Value = Value;
        return field;
    }

    public void ExpandFill(bool vertical = false)
    {
        SizeFlagsHorizontal = SizeFlags.ExpandFill;
        if (vertical)
            SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    public void SetValue(string value)
    {
        Value = value;
    }

    /* Private methods. */
    private void OnChanged()
    {
        Godot.GD.Print("FCUK TEXTAREA");
        Changed?.Invoke(this);
    }
}
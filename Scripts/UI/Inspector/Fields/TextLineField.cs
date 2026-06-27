using Godot;
using System;

namespace Rusty.ISA;

/// <summary>
/// A text line field.
/// </summary>
[GlobalClass]
public partial class TextLineField : HBoxContainer, IWidget, IValued<string>
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
            LineEdit.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo { get; set; }

    public string Value
    {
        get => LineEdit.Text;
        set => LineEdit.Text = value;
    }

    /* Private methods. */
    private Label Label { get; set; }
    private LineEdit LineEdit { get; set; }

    /* Public events. */
    public event Action<IWidget> Changed;

    /* Constructors. */
    public TextLineField()
    {
        Label = new();
        AddChild(Label);
        TitleWidth = 160;

        LineEdit = new();
        AddChild(LineEdit);
        LineEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        LineEdit.TextChanged += OnChanged;
    }

    /* Public methods. */
    public IWidget Copy()
    {
        TextLineField field = new();
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
    private void OnChanged(string text)
    {
        Godot.GD.Print("FCUK TEXTLINE");
        Changed?.Invoke(this);
    }
}
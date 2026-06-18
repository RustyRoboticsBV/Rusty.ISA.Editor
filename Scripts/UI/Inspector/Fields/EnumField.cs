using Godot;
using System;

namespace Rusty.ISA;

/// <summary>
/// An enum field.
/// </summary>
public partial class EnumField : HBoxContainer, IWidget, IValued<int>
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
            OptionButton.TooltipText = value;
        }
    }

    public string[] Choices
    {
        get => OptionsCache;
        set
        {
            OptionsCache = value;
            OptionButton.Clear();
            for (int i = 0; i < value.Length; i++)
            {
                OptionButton.AddItem(value[i]);
            }

            if (value.Length == 0)
                Value = -1;
            else
                Value = 0;
        }
    }
    public int Value
    {
        get => OptionButton.Selected;
        set
        {
            OptionButton.Selected = value;
            OnChanged(Value);
        }
    }

    /* Private methods. */
    private Label Label { get; set; }
    private OptionButton OptionButton { get; set; }
    private string[] OptionsCache { get; set; } = [];

    /* Public events. */
    public event Action<IWidget> Changed;

    /* Constructors. */
    public EnumField()
    {
        Label = new();
        AddChild(Label);
        TitleWidth = 160;

        OptionButton = new();
        OptionButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        OptionButton.ItemSelected += OnChanged;
        AddChild(OptionButton);

        Value = -1;
    }

    /* Public methods. */
    public IWidget Copy()
    {
        EnumField field = new();
        field.SizeFlagsHorizontal = SizeFlagsHorizontal;
        field.SizeFlagsVertical = SizeFlagsVertical;
        field.Title = Title;
        field.TitleWidth = TitleWidth;
        field.Description = Description;
        field.Choices = Choices;
        field.Value = Value;
        return field;
    }

    public void ExpandFill(bool vertical = false)
    {
        SizeFlagsHorizontal = SizeFlags.ExpandFill;
        if (vertical)
            SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    /* Private methods. */
    private void OnChanged(long index)
    {
        Godot.GD.Print("FCUK ENUM");
        Changed?.Invoke(this);
    }
}
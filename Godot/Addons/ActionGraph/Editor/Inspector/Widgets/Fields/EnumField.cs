using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An enum field.
/// </summary>
internal sealed partial class EnumField : HBoxContainer, IField<EnumField>
{
    /* Public properties. */
    public string TitleText
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    public int TitleWidth
    {
        get => (int)Label.CustomMinimumSize.X;
        set => Label.CustomMinimumSize = new(value, Label.CustomMinimumSize.Y);
    }
    public new string TooltipText
    {
        get => base.TooltipText;
        set
        {
            base.TooltipText = value;
            Label.TooltipText = value;
            OptionButton.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo
    {
        get => OptionButton.UndoRedo;
        set => OptionButton.UndoRedo = value;
    }

    public int Selected => OptionButton.Selected;

    /* Private methods. */
    private Label Label { get; set; }
    private OptionButton OptionButton { get; set; }

    /* Public events. */
    public event Action StateChanged;

    /* Constructors. */
    public EnumField()
    {
        Label = new();
        AddChild(Label, false, InternalMode.Front);
        TitleWidth = 160;

        OptionButton = new();
        OptionButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        OptionButton.Selected = -1;
        OptionButton.ItemSelected += (index) => StateChanged?.Invoke();
        AddChild(OptionButton, false, InternalMode.Front);
    }

    public EnumField(string[] items) : this() => OptionButton.SetItems(items);

    public EnumField(string[] items, int selected) : this(items) => OptionButton.Selected = selected;

    public EnumField(string title, string[] items, int selected) : this(items, selected) => TitleText = title;

    /* Public methods. */
    public EnumField DuplicateWidget()
    {
        EnumField field = Duplicate() as EnumField;
        field.TitleText = TitleText;
        field.TitleWidth = TitleWidth;
        field.TooltipText = TooltipText;
        field.UndoRedo = UndoRedo;
        field.SetItems(GetItems());
        field.OptionButton.Selected = Selected;
        return field;
    }

    public void SetItems(string[] items) => OptionButton.SetItems(items);
    public string[] GetItems() => OptionButton.GetItems();

    /// <summary>
    /// Change the selected item without recording it in undo/redo.
    /// </summary>
    public void SetSelected(int index) => OptionButton.Select(index);
    /// <summary>
    /// Change the selected item and record it in undo/redo.
    /// </summary>
    public void CommitSelected(int index) => OptionButton.CommitSelect(index);
    /// <summary>
    /// If an edit is in progress, cancel it and revert to the last committed selected item.
    /// </summary>
    public void CancelSelected() => OptionButton.CancelSelect();
}
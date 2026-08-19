using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A choice widget.
/// </summary>
internal sealed partial class ChoiceWidget : VBoxContainer, IWidget<ChoiceWidget>
{
    /* Public properties. */
    public string TitleText
    {
        get => Enum.TitleText;
        set => Enum.TitleText = value;
    }
    public Control Selected => Enum.Selected != -1 ? Items[Enum.Selected] : null;
    public int Indentation
    {
        get => Margin.GetThemeConstant("margin_left");
        set => Margin.AddThemeConstantOverride("margin_left", value);
    }
    public UndoRedo UndoRedo
    {
        get => Enum.UndoRedo;
        set
        {
            Enum.UndoRedo = value;
            foreach (Control item in Items)
            {
                if (item is IWidget widget)
                    widget.UndoRedo = value;
            }
        }
    }

    /* Private properties. */
    private EnumField Enum { get; set; }
    private MarginContainer Margin { get; set; }
    private Control[] Items { get; set; }

    /* Public methods. */
    public event Action StateChanged;

    /* Constructors. */
    public ChoiceWidget(string titleText, Dictionary<string, Control> choices, int selected)
    {
        MouseFilter = MouseFilterEnum.Pass;
        SizeFlagsHorizontal = SizeFlags.ExpandFill;

        // Add option button.
        Enum = new(titleText, choices.Keys.ToArray(), selected);
        Enum.Name = "EnumField";
        Enum.StateChanged += () => StateChanged?.Invoke(); 
        AddChild(Enum);

        // Add items.
        Margin = new();
        Margin.Name = "ChoiceMargin";
        AddChild(Margin);
        Indentation = 28;

        Items = choices.Values.ToArray();
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].Visible = selected == i;
            Margin.AddChild(Items[i]);

            if (Items[i] is IWidget widget)
                widget.StateChanged += () => StateChanged?.Invoke();
        }
    }

    /* Public methods. */
    IWidget IWidget.DuplicateWidget() => DuplicateWidget();

    public ChoiceWidget DuplicateWidget()
    {
        Dictionary<string, Control> items = new();
        string[] itemNamesCache = Enum.GetItems();
        for (int i = 0; i < Items.Length; i++)
        {
            Control optional = Items[i] is IWidget widget
                ? widget.DuplicateWidget() as Control
                : Items[i].Duplicate() as Control;
            items.Add(itemNamesCache[i], optional);
        }

        ChoiceWidget copy = new(TitleText, items, Enum.Selected);
        copy.SizeFlagsHorizontal = SizeFlagsHorizontal;
        copy.SizeFlagsVertical = SizeFlagsVertical;
        copy.Indentation = Indentation;
        copy.UndoRedo = UndoRedo;

        return copy;
    }

    public void Select(int index)
    {
        Enum.CommitSelected(index);
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].Visible = Enum.Selected == i;
        }
    }
}
using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph;

/// <summary>
/// A foldable vertical widget group.
/// </summary>
[GlobalClass]
public partial class FoldoutGroup : FoldableContainer, IWidget, IGroup
{
    /* Public properties. */
    public int TitleWidth { get; set; } // TODO: this doesn't do anything for this group, refactor?
    public string Description
    {
        get => TooltipText;
        set
        {
            TooltipText = value;
            Margin.TooltipText = value;
            Inner.TooltipText = value;
        }
    }
    public int Indentation
    {
        get => Margin.GetThemeConstant("margin_left");
        set => Margin.AddThemeConstantOverride("margin_left", value);
    }
    public UndoRedo UndoRedo { get; set; }

    public List<IWidget> Children { get; private set; } = new();

    /* Private methods. */
    private MarginContainer Margin { get; set; }
    private VBoxContainer Inner { get; set; }

    /* Public events. */
    public event Action<IWidget> Changed;

    /* Constructors. */
    public FoldoutGroup()
    {
        Margin = new();
        Margin.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Margin.SizeFlagsVertical = SizeFlags.ExpandFill;
        AddChild(Margin);

        Inner = new();
        Inner.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Inner.SizeFlagsVertical = SizeFlags.ExpandFill;
        Margin.AddChild(Inner);

        Folded = true;
    }

    /* Public methods. */
    public IWidget Copy()
    {
        FoldoutGroup group = new();
        group.SizeFlagsHorizontal = SizeFlagsHorizontal;
        group.SizeFlagsVertical = SizeFlagsVertical;
        group.Title = Title;
        group.TitleWidth = TitleWidth;
        group.Description = Description;
        group.Indentation = Indentation;
        foreach (IWidget child in Children)
        {
            group.AddWidget(child.Copy());
        }
        return group;
    }

    public void ExpandFill(bool vertical = false)
    {
        SizeFlagsHorizontal = SizeFlags.ExpandFill;
        if (vertical)
            SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    public void AddWidget(IWidget widget)
    {
        Inner.AddChild(widget as Node);
        Children.Add(widget);
        widget.Changed += OnChanged;
        OnChanged(widget);
    }

    public void CancelFocus() { }

    /* Private methods. */
    private void OnChanged(IWidget widget)
    {
        Changed?.Invoke(this);
    }
}
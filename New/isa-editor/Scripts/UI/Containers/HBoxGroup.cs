using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ISA;

/// <summary>
/// A horizontal widget group.
/// </summary>
public partial class HBoxGroup : VBoxContainer, IWidget, IGroup
{
    /* Public properties. */
    public string Title
    {
        get => Label.Text;
        set
        {
            Label.Text = value;
            Label.Visible = !string.IsNullOrEmpty(Label.Text);
        }
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
            Inner.TooltipText = value;
        }
    }
    public int Indentation
    {
        get => Margin.GetThemeConstant("margin_left");
        set => Margin.AddThemeConstantOverride("margin_left", value);
    }

    public List<IWidget> Children { get; private set; } = new();

    /* Private methods. */
    private Label Label { get; set; }
    private MarginContainer Margin { get; set; }
    private HBoxContainer Inner { get; set; }

    /* Public events. */
    public event Action<IWidget> Changed;

    /* Constructors. */
    public HBoxGroup()
    {
        Label = new();
        AddChild(Label);
        Label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Label.Visible = false;

        Margin = new();
        AddChild(Margin);
        Margin.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Margin.SizeFlagsVertical = SizeFlags.ExpandFill;

        Inner = new();
        Margin.AddChild(Inner);
        Inner.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Inner.SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    /* Public methods. */
    public IWidget Copy()
    {
        HBoxGroup group = new();
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

    /* Private methods. */
    private void OnChanged(IWidget widget)
    {
        Godot.GD.Print("FCUK HBOX");
        Changed?.Invoke(this);
    }
}
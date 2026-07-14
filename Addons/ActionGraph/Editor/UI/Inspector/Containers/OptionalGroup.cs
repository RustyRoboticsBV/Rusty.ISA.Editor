using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph;

/// <summary>
/// A vertical widget group with a toggle.
/// </summary>
[GlobalClass]
public partial class OptionalGroup : VBoxContainer, IWidget, IGroup, IValued<bool>
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
    public bool Value
    {
        get => CheckBox.ButtonPressed;
        set
        {
            CheckBox.ButtonPressed = value;
            UpdateElementVisibility();
        }
    }
    public UndoRedo UndoRedo { get; set; }

    public List<IWidget> Children { get; private set; } = new();

    /* Private methods. */
    private Label Label { get; set; }
    private CheckBox CheckBox { get; set; }
    private MarginContainer Margin { get; set; }
    private VBoxContainer Inner { get; set; }

    /* Public events. */
    public event Action<IWidget> Changed;

    /* Constructors. */
    public OptionalGroup()
    {
        HBoxContainer hbox = new();
        hbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(hbox);

        Label = new();
        hbox.AddChild(Label);
        Label.Visible = false;

        CheckBox = new();
        CheckBox.Pressed += OnPressed;
        hbox.AddChild(CheckBox);

        Margin = new();
        Margin.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Margin.SizeFlagsVertical = SizeFlags.ExpandFill;
        AddChild(Margin);

        Inner = new();
        Inner.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Inner.SizeFlagsVertical = SizeFlags.ExpandFill;
        Margin.AddChild(Inner);
    }

    /* Public methods. */
    public IWidget Copy()
    {
        OptionalGroup group = new();
        group.SizeFlagsHorizontal = SizeFlagsHorizontal;
        group.SizeFlagsVertical = SizeFlagsVertical;
        group.Title = Title;
        group.TitleWidth = TitleWidth;
        group.Description = Description;
        group.Indentation = Indentation;
        group.Value = Value;
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
        widget.Visible = Value;
        widget.Changed += OnChanged;
        OnChanged(widget);
    }

    public void SetValue(bool value)
    {
        Value = value;
    }

    public void CancelFocus() { }

    /* Private methods. */
    private void OnPressed()
    {
        UpdateElementVisibility();
        Changed?.Invoke(this);
    }

    private void OnChanged(IWidget widget)
    {
        Changed?.Invoke(this);
    }

    private void UpdateElementVisibility()
    {
        foreach (IWidget widget in Children)
        {
            widget.Visible = Value;
        }
    }
}
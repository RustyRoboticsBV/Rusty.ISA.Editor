using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public partial class GraphNode : Godot.GraphNode, IGraphElement
{
    /* Public properties. */
    public GraphEdit GraphEdit
    {
        get
        {
            Node parent = GetParent();
            if (parent is GraphEdit graphEdit)
                return graphEdit;
            else
                return null;
        }
    }
    public GraphFrame Frame { get; set; }

    public Color TitleColor { get; set; } = new Color(0.5f, 0.5f, 0.5f);
    public string TitleText
    {
        get => TitleLabel.Text;
        set => TitleLabel.Text = value;
    }
    public Texture2D TitleIcon
    {
        get => TitleTextureRect.Texture;
        set => TitleTextureRect.Texture = value;
    }

    /* Private properties. */
    private Label TitleLabel { get; set; }
    private TextureRect TitleTextureRect { get; set; }
    private List<SlotLabels> SlotLabels { get; } = new();
    private MarginContainer PreviewContainer { get; set; }
    private Label Preview { get; set; }
    private Control BottomMargin { get; set; }

    /* Public events. */
    public new event Action<IGraphElement> NodeSelected;
    public new event Action<IGraphElement> NodeDeselected;
    public new event Action<IGraphElement> Dragged;
    public new event Action<IGraphElement> DeleteRequest;

    /* Constructors. */
    public GraphNode()
    {
        // Set default minimum size.
        CustomMinimumSize = new Vector2(160, 80);

        // Replace title elements.
        HBoxContainer titleContainer = GetChild(0, true) as HBoxContainer;
        titleContainer.SizeFlagsHorizontal = SizeFlags.Fill;
        titleContainer.CustomMinimumSize = new(0f, 40f);
        titleContainer.RemoveChild(titleContainer.GetChild(0, true));

        MarginContainer titleMargin = new();
        titleMargin.AddThemeConstantOverride("margin_left", 4);
        titleMargin.AddThemeConstantOverride("margin_right", 4);
        titleContainer.AddChild(titleMargin);

        HBoxContainer titleSpacer = new();
        titleMargin.AddChild(titleSpacer);

        TitleTextureRect = new();
        TitleTextureRect.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        titleSpacer.AddChild(TitleTextureRect);

        TitleLabel = new();
        titleSpacer.AddChild(TitleLabel);
        TitleText = "New Node";

        // Add top margin.
        Control marginTop = new();
        marginTop.CustomMinimumSize = new(0f, 5f);
        AddChild(marginTop);

        // Add default ports.
        SetInputPort(0, "");
        SetOutputPort(0, "");

        // Add preview label.
        PreviewContainer = new();
        PreviewContainer.AddThemeConstantOverride("margin_left", 8);
        PreviewContainer.AddThemeConstantOverride("margin_right", 8);
        PreviewContainer.AddThemeConstantOverride("margin_bottom", 4);
        PreviewContainer.AddThemeConstantOverride("margin_top", 4);
        AddChild(PreviewContainer);
        PreviewContainer.Hide();

        Preview = new();
        PreviewContainer.AddChild(Preview);

        // Add bottom margin.
        BottomMargin = new();
        BottomMargin.CustomMinimumSize = new(0f, 5f);
        AddChild(BottomMargin);
        BottomMargin.Name = "Bottom Margin";

        // Set default values.
        AddThemeColorOverride("font_color", Colors.White);
        AddThemeStyleboxOverride("panel", new StyleBoxFlat() { BgColor = new Color(0.13f, 0.13f, 0.13f) });
        AddThemeStyleboxOverride("panel_selected", new StyleBoxFlat() { BgColor = new Color(0.13f, 0.13f, 0.13f) });
        AddThemeStyleboxOverride("titlebar_selected", new StyleBoxFlat() { BgColor = Colors.White });

        // Subscribe to events.
        base.NodeSelected += OnNodeSelected;
        base.NodeDeselected += OnNodeDeselected;
        base.Dragged += OnDragged;
        base.DeleteRequest += OnDeleteRequest;
    }

    /* Public methods. */
    public bool IsNestedIn(GraphFrame frame)
    {
        return Frame == frame || Frame != null && Frame.IsNestedIn(frame);
    }

    public void RequestDelete()
    {
        DeleteRequest?.Invoke(this);
    }

    /// <summary>
    /// Add an input port.
    /// </summary>
    public void SetInputPort(int index, string text)
    {
        // Ensure that enough slots exist.
        EnsureSlot(index);

        // Enable slot.
        SetSlot(index + 1,
            true, GetSlotTypeLeft(index + 1), GetSlotColorLeft(index + 1),
            IsSlotEnabledRight(index + 1), GetSlotTypeRight(index + 1), GetSlotColorRight(index + 1)
        );

        // Set slot label.
        SlotLabels[index].InputText = text;
    }

    /// <summary>
    /// Add an output port.
    /// </summary>
    public void SetOutputPort(int index, string text)
    {
        // Ensure that enough slots exist.
        EnsureSlot(index);

        // Enable slot.
        SetSlot(index + 1,
            IsSlotEnabledLeft(index + 1), GetSlotTypeLeft(index + 1), GetSlotColorLeft(index + 1),
            true, GetSlotTypeRight(index + 1), GetSlotColorRight(index + 1)
        );

        // Set slot label.
        SlotLabels[index].OutputText = text;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        // Change title color depending on if we're selected or not.
        AddThemeStyleboxOverride("titlebar", new StyleBoxFlat() { BgColor = TitleColor });

        if (Selected)
        {
            TitleTextureRect.Modulate = Colors.Gray;
            TitleLabel.Modulate = Colors.Gray;
        }
        else
        {
            TitleTextureRect.Modulate = Colors.White;
            TitleLabel.Modulate = Colors.White;
        }

        // Hide preview if it's empty.
        PreviewContainer.Visible = Preview.Text != "";

        // Shrink to minimum size.
        Rect2 totalRect = new();
        foreach (Node child in GetChildren())
        {
            if (child is Control control && control.Visible)
                totalRect = totalRect.Merge(new Rect2(control.Position, control.Size));
        }
        Size = new(0, totalRect.Size.Y);
    }

    /* Private methods. */
    /// <summary>
    /// Ensure a number of input/output slot pairs. We don't enable them by default.
    /// </summary>
    private void EnsureSlot(int index)
    {
        if (SlotLabels.Count > index)
            return;

        // Temporarily remove the preview & bottom margin.
        if (PreviewContainer != null && BottomMargin != null)
        {
            RemoveChild(PreviewContainer);
            RemoveChild(BottomMargin);
        }

        // Add slots until we have enough.
        while (SlotLabels.Count <= index)
        {
            SlotLabels labels = new();
            labels.CustomMinimumSize = new(0f, 25f);
            labels.MouseFilter = MouseFilterEnum.Ignore;
            SlotLabels.Add(labels);
            AddChild(labels);
        }

        // Add the preview back as the last child.
        if (PreviewContainer != null && BottomMargin != null)
        {
            AddChild(PreviewContainer);
            AddChild(BottomMargin);
        }
    }

    private void OnNodeSelected()
    {
        NodeSelected?.Invoke(this);
    }

    private void OnNodeDeselected()
    {
        NodeDeselected?.Invoke(this);
    }

    private void OnDeleteRequest()
    {
        RequestDelete();
    }

    private void OnDragged(Vector2 from, Vector2 to)
    {
        Dragged?.Invoke(this);
    }
}
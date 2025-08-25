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
    public Vector2 UnsnappedPosition { get; set; }

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
    public string PreviewText
    {
        get => Preview.Text;
        set
        {
            if (Preview.Text != value)
            {
                Preview.Text = value;
                PreviewContainer.Visible = Preview.Text != "";
                ScheduledFrameUpdate = true;
            }
        }
    }

    /* Private properties. */
    private Label TitleLabel { get; set; }
    private TextureRect TitleTextureRect { get; set; }
    private List<SlotLabels> SlotLabels { get; } = new();
    private MarginContainer PreviewContainer { get; set; }
    private Label Preview { get; set; }
    private Control BottomMargin { get; set; }
    private bool ScheduledFrameUpdate { get; set; }

    private static int FontSize => 13;

    private bool IsClicked { get; set; }

    /* Public events. */
    public event Action<IGraphElement> MouseClicked;
    public event Action<IGraphElement> MouseDragged;
    public event Action<IGraphElement> MouseReleased;

    /* Constructors. */
    public GraphNode()
    {
        // Set default minimum size and position.
        CustomMinimumSize = new Vector2(160, 80);
        UnsnappedPosition = PositionOffset;

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
        marginTop.MouseFilter = MouseFilterEnum.Ignore;
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
        Preview.AddThemeFontSizeOverride("font_size", FontSize + 1);
        PreviewContainer.AddChild(Preview);

        // Add bottom margin.
        BottomMargin = new();
        BottomMargin.CustomMinimumSize = new(0f, 5f);
        BottomMargin.MouseFilter = MouseFilterEnum.Ignore;
        AddChild(BottomMargin);
        BottomMargin.Name = "Right Margin";

        // Set default values.
        AddThemeColorOverride("font_color", Colors.White);
        AddThemeStyleboxOverride("panel", new StyleBoxFlat() { BgColor = new Color(0.13f, 0.13f, 0.13f) });
        AddThemeStyleboxOverride("panel_selected", new StyleBoxFlat() { BgColor = new Color(0.13f, 0.13f, 0.13f) });
        AddThemeStyleboxOverride("titlebar_selected", new StyleBoxFlat() { BgColor = Colors.White });

        // Force one update.
        _Process(0);
    }

    /* Public methods. */
    public override string ToString()
    {
        return $"Node (element index {GetIndex()}): \"{TitleText}\"";
    }

    public void MoveTo(Vector2 position)
    {
        UnsnappedPosition = position;
        Snapper.SnapToGrid(this);
    }

    public bool IsNestedIn(GraphFrame frame)
    {
        return Frame == frame || Frame != null && Frame.IsNestedIn(frame);
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

        // Shrink to minimum size.
        Rect2 totalRect = new();
        foreach (Node child in GetChildren())
        {
            if (child is Control control && control.Visible)
                totalRect = totalRect.Merge(new Rect2(control.Position, control.Size));
        }
        Size = new(0, totalRect.Size.Y);

        // Update frame if necessary.
        if (ScheduledFrameUpdate)
        {
            ScheduledFrameUpdate = false;
            Frame?.UpdateSizeAndPosition();
        }

        // Snap to grid.
        Snapper.SnapToGrid(this);
    }

    public override void _GuiInput(InputEvent @event)
    {
        // Suppress built-in drag & selection.
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                IsClicked = true;
                MouseClicked?.Invoke(this);
                AcceptEvent();
                return;
            }
            else if (!mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left && IsClicked)
            {
                IsClicked = false;
                MouseReleased?.Invoke(this);
                AcceptEvent();
                return;
            }
        }
        else if (@event is InputEventMouseMotion && IsClicked)
        {
            MouseDragged?.Invoke(this);
            AcceptEvent();
            return;
        }

        // Otherwise, let normal controls still work.
        base._GuiInput(@event);
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
            labels.FontSize = FontSize;
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
}
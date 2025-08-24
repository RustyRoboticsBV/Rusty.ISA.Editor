using Godot;
using System;

namespace Rusty.ISA.Editor;

/// <summary>
/// An ISA editor graph comment.
/// </summary>
public partial class GraphComment : Godot.GraphNode, IGraphElement
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

    public string CommentText
    {
        get => Label.Text;
        set
        {
            if (Label.Text != value)
            {
                Label.Text = value;
                ScheduledFrameUpdate = true;
            }
        }
    }
    public Color BgColor { get; set; } = new Color(0.13f, 0.13f, 0.13f, 0.75f);
    public Color TextColor { get; set; } = new Color(0f, 1f, 0f, 1f);

    /* Public events. */
    public new event Action<IGraphElement> NodeSelected;
    public new event Action<IGraphElement> NodeDeselected;
    public new event Action<IGraphElement> Dragged;
    public new event Action<IGraphElement> DeleteRequest;

    /* Private properties. */
    private MarginContainer LabelMargin { get; set; }
    private RichTextLabel Label { get; set; }

    private bool ScheduledFrameUpdate { get; set; }

    /* Constructors. */
    public GraphComment()
    {
        // Set default minimum size.
        CustomMinimumSize = new(200f, 40f);

        // Add contents.
        LabelMargin = new();
        LabelMargin.AddThemeConstantOverride("margin_left", 16);
        LabelMargin.AddThemeConstantOverride("margin_right", 16);
        LabelMargin.AddThemeConstantOverride("margin_bottom", 8);
        LabelMargin.AddThemeConstantOverride("margin_top", 8);
        AddChild(LabelMargin);

        Label = new()
        {
            Name = "New comment.",
            SizeFlagsHorizontal = SizeFlags.ShrinkBegin,
            SizeFlagsVertical = SizeFlags.ExpandFill,
            FitContent = true,
            AutowrapMode = TextServer.AutowrapMode.Off,
            MouseFilter = MouseFilterEnum.Pass
        };
        LabelMargin.AddChild(Label);

        // Add style overrides.
        AddThemeStyleboxOverride("panel_selected", new StyleBoxFlat()
        {
            BgColor = EditorNodeInfo.SelectedMainColor
        });
        AddThemeStyleboxOverride("titlebar_selected", new StyleBoxFlat()
        {
            BgColor = EditorNodeInfo.SelectedMainColor
        });

        // Empty the title container.
        if (GetChild(0, true) is Control titleContainer)
        {
            while (titleContainer.GetChildCount(true) > 0)
            {
                titleContainer.RemoveChild(titleContainer.GetChild(0, true));
            }
        }

        // Subscribe to events.
        base.NodeSelected += OnNodeSelected;
        base.NodeDeselected += OnNodeDeselected;
        base.Dragged += OnDragged;
        base.DeleteRequest += OnDeleteRequest;
    }

    /* Public methods. */
    public override string ToString()
    {
        return $"Comment (element index {GetIndex()}): \"{CommentText}\"";
    }

    public bool IsNestedIn(GraphFrame frame)
    {
        return Frame == frame || Frame != null && Frame.IsNestedIn(frame);
    }

    public void RequestDelete()
    {
        DeleteRequest?.Invoke(this);
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        // Update background color.
        AddThemeStyleboxOverride("panel", new StyleBoxFlat()
        {
            BgColor = BgColor
        });
        AddThemeStyleboxOverride("titlebar", new StyleBoxFlat()
        {
            BgColor = BgColor
        });

        // Update label text color.
        if (Label != null)
        {
            Label.Size = Vector2.Zero;
            Label.AddThemeColorOverride("default_color", Selected ? EditorNodeInfo.SelectedTextColor : TextColor);
        }

        // Shrink to minimum size.
        Size = Vector2.Zero;

        // Update frame.
        if (ScheduledFrameUpdate)
        {
            ScheduledFrameUpdate = false;
            Frame?.UpdateSizeAndPosition();
        }
    }

    /* Private methods. */
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
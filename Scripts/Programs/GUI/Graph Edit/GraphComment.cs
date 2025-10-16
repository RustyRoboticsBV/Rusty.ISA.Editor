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
    public Vector2 UnsnappedPosition { get; set; }

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
    public event Action<IGraphElement> MouseClicked;
    public event Action<IGraphElement> MouseDragged;
    public event Action<IGraphElement> MouseReleased;

    /* Private properties. */
    private MarginContainer LabelMargin { get; set; }
    private RichTextLabel Label { get; set; }

    private bool ScheduledFrameUpdate { get; set; }

    private bool IsClicked { get; set; }

    /* Constructors. */
    public GraphComment()
    {
        // Set default minimum size.
        CustomMinimumSize = new(200f, 40f);
        UnsnappedPosition = PositionOffset;

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
    }

    /* Public methods. */
    public override string ToString()
    {
        return $"Comment (element index {GetIndex()}): \"{CommentText}\"";
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
            Frame?.FitAroundElements();
        }
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
}
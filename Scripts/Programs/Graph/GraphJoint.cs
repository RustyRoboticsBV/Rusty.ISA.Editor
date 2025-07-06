using Godot;
using System;

namespace Rusty.ISA.Editor;

/// <summary>
/// An ISA editor graph joint.
/// </summary>
public partial class GraphJoint : Godot.GraphNode, IGraphElement
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

    public Color BgColor { get; set; } = new Color(0.13f, 0.13f, 0.13f);

    /* Public events. */
    public new event Action<IGraphElement> NodeSelected;
    public new event Action<IGraphElement> NodeDeselected;
    public new event Action<IGraphElement> Dragged;
    public new event Action<IGraphElement> DeleteRequest;

    /* Private properties. */
    private RichTextLabel Label { get; set; }

    /* Constructors. */
    public GraphJoint()
    {
        // Set default minimum size.
        CustomMinimumSize = new(60f, 40f);

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

        // Add slots.
        Control slots = new();
        slots.MouseFilter = MouseFilterEnum.Ignore;
        slots.CustomMinimumSize = CustomMinimumSize;
        AddChild(slots);
        SetSlotEnabledLeft(0, true);
        SetSlotEnabledRight(0, true);

        // Subscribe to events.
        base.NodeSelected += OnNodeSelected;
        base.NodeDeselected += OnNodeDeselected;
        base.Dragged += OnDragged;
        base.DeleteRequest += OnDeleteRequest;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        // Update background color.
        AddThemeStyleboxOverride("panel", new StyleBoxFlat()
        {
            BgColor = Selected ? EditorNodeInfo.SelectedMainColor : BgColor
        });
        AddThemeStyleboxOverride("titlebar", new StyleBoxFlat()
        {
            BgColor = Selected ? EditorNodeInfo.SelectedMainColor : BgColor
        });

        // Shrink to minimum size.
        Size = Vector2.Zero;
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
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

    /* Private propertise. */
    private Label Slots { get; set; }

    /* Public events. */
    public new event Action<IGraphElement> DeleteRequest;

    // TODO: remove
    public new event Action<IGraphElement> NodeSelected;
    public new event Action<IGraphElement> NodeDeselected;
    public new event Action<IGraphElement> Dragged;
    // END TODO

    /* Private properties. */
    private RichTextLabel Label { get; set; }

    /* Constructors. */
    public GraphJoint()
    {
        // Set default minimum size.
        CustomMinimumSize = new(40f, 40f);

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
        Slots = new();
        Slots.MouseFilter = MouseFilterEnum.Stop;
        Slots.CustomMinimumSize = CustomMinimumSize - new Vector2(16f, 0f);
        Slots.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
        Slots.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        Slots.HorizontalAlignment = HorizontalAlignment.Center;
        Slots.VerticalAlignment = VerticalAlignment.Center;
        AddChild(Slots);
        SetSlotEnabledLeft(0, true);
        SetSlotEnabledRight(0, true);
    }

    /* Public methods. */
    public override string ToString()
    {
        return $"Joint (element index {GetIndex()})";
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
            BgColor = Selected ? EditorNodeInfo.SelectedMainColor : BgColor
        });
        AddThemeStyleboxOverride("titlebar", new StyleBoxFlat()
        {
            BgColor = Selected ? EditorNodeInfo.SelectedMainColor : BgColor
        });

        // Shrink to minimum size.
        Size = Vector2.Zero;
    }
}
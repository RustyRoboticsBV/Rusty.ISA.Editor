using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public partial class GraphFrame : Godot.GraphFrame, IGraphElement
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
    public int ID { get; set; } = -1;

    /* Private properties. */
    private List<IGraphElement> Elements { get; } = new();
    private Vector2 LastOffset { get; set; }
    private List<Vector2> ElementOffsets { get; } = new();

    /* Public events. */
    public new event Action<IGraphElement> NodeSelected;
    public new event Action<IGraphElement> NodeDeselected;
    public new event Action<IGraphElement> Dragged;
    public new event Action<IGraphElement> DeleteRequest;

    /* Constructors. */
    public GraphFrame()
    {
        // Set default minimum size.
        CustomMinimumSize = new Vector2(160, 160);

        // Enable/disable frame features.
        AutoshrinkEnabled = false;
        TintColorEnabled = true;

        // Set defaults.
        Title = "New Frame";
        TintColor = new(0.123f, 0.123f, 0.123f);

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
        // Move all elements to the parent frame (if there is one).
        for (int i = 0; i < Elements.Count; i++)
        {
            Elements[i].Frame = null;
            if (Frame != null)
                Frame.AddElement(Elements[i]);
        }

        // Call delete event.
        DeleteRequest?.Invoke(this);
    }

    /// <summary>
    /// Add a graph element to this frame. This removes it from its old frame, it is was attached to one.
    /// </summary>
    public void AddElement(IGraphElement element)
    {
        if (element == this)
            return;

        // Remove element from old frame.
        if (element.Frame != null)
            element.Frame.RemoveElement(element);

        // Add element to new frame.
        Elements.Add(element);
        element.Frame = this;
        element.Dragged += OnElementDragged;
        element.DeleteRequest += OnElementDeleted;

        // Alter position & size.
        UpdateSizeAndPosition();
    }

    /// <summary>
    /// Remove an element from this frame.
    /// </summary>
    public void RemoveElement(IGraphElement element)
    {
        // Remove.
        Elements.Remove(element);
        element.Frame = null;
        element.Dragged -= OnElementDragged;

        // Alter position & size.
        UpdateSizeAndPosition();
    }

    /// <summary>
    /// Update the frame's size and position.
    /// </summary>
    public void UpdateSizeAndPosition()
    {
        if (Elements.Count == 0)
        {
            Size = Vector2.Zero;
            return;
        }

        // Figure out new bounds.
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;
        for (int i = 0; i < Elements.Count; i++)
        {
            float elementMinX = Elements[i].PositionOffset.X;
            if (elementMinX < minX)
                minX = elementMinX;

            float elementMinY = Elements[i].PositionOffset.Y;
            if (elementMinY < minY)
                minY = elementMinY;

            float elementMaxX = elementMinX + Elements[i].Size.X;
            if (elementMaxX > maxX)
                maxX = elementMaxX;

            float elementMaxY = elementMinY + Elements[i].Size.Y;
            if (elementMaxY > maxY)
                maxY = elementMaxY;
        }

        minX -= AutoshrinkMargin;
        maxX += AutoshrinkMargin;
        minY -= AutoshrinkMargin;
        maxY += AutoshrinkMargin;

        // Apply new bounds.
        PositionOffset = new(minX, minY);
        Size = new(maxX - minX, maxY - minY);

        // Store member offsets.
        ElementOffsets.Clear();
        foreach (IGraphElement element in Elements)
        {
            ElementOffsets.Add(element.PositionOffset - PositionOffset);
        }

        // Also update parent frame.
        if (Frame != null)
            Frame.UpdateSizeAndPosition();
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        // Update alpha.
        float alpha = Selected ? 0.75f : 0.5f;
        TintColor = new(TintColor.R, TintColor.G, TintColor.B, alpha);

        // If we move the frame, also move members with it.
        if (LastOffset != PositionOffset)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].PositionOffset = PositionOffset + ElementOffsets[i];
            }
            LastOffset = PositionOffset;
        }

        // Update drag margin.
        // We use the smallest of the size axes' divided by two, because otherwise Godot will spam the 
        // console with internal error logs if the drag margin exceeds the half-size on one of the two
        // axes.
        // TODO: Refactor if this ever gets fixed, because this means that our drag area won't encompass
        // the entire frame if it's not perfectly square.
        DragMargin = Mathf.FloorToInt(Mathf.Min(Size.X, Size.Y) / 2f);
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

    private void OnDragged(Vector2 from, Vector2 to)
    {
        Dragged?.Invoke(this);
        LastOffset = to;
    }

    private void OnDeleteRequest()
    {
        RequestDelete();
    }

    private void OnElementDragged(IGraphElement element)
    {
        if (!Selected)
            UpdateSizeAndPosition();
    }

    private void OnElementDeleted(IGraphElement element)
    {
        RemoveElement(element);
    }
}
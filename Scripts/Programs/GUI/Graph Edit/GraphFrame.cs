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
    public event Action<IGraphElement> MouseClicked;
    public event Action<IGraphElement> MouseDragged;
    public event Action<IGraphElement> MouseReleased;

    /* Private properties. */
    private bool IsClicked { get; set; }

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
    }

    /* Public methods. */
    public override string ToString()
    {
        return $"Frame (element index {GetIndex()}): \"{Title}\"";
    }

    public bool IsNestedIn(GraphFrame frame)
    {
        return Frame == frame || Frame != null && Frame.IsNestedIn(frame);
    }

    /// <summary>
    /// Add a graph element to this frame. This removes it from its old frame, it is was attached to one.
    /// </summary>
    public void AddElement(IGraphElement element)
    {
        if (element == this || element == Frame)
            return;

        // Remove element from old frame.
        if (element.Frame != null)
            element.Frame.RemoveElement(element);

        // Add element to new frame.
        Elements.Add(element);
        element.Frame = this;

        // Recalculate and apply new position & size.
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
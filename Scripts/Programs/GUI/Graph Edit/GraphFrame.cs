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
    public Vector2 UnsnappedPosition { get; set; }
    public Vector2 UnsnappedSize { get; set; }
    public int ID { get; set; } = -1;
    public List<IGraphElement> Elements { get; } = new();

    /* Private properties. */
    private Vector2 LastOffset { get; set; }
    private List<Vector2> ElementOffsets { get; } = new();
    private Control SelectionControl { get; set; }

    /* Public events. */
    public event Action<IGraphElement> MouseClicked;
    public event Action<IGraphElement> MouseDragged;
    public event Action<IGraphElement> MouseReleased;

    /* Private properties. */
    private bool IsClicked { get; set; }

    /* Constructors. */
    public GraphFrame()
    {
        // Set default minimum size and position.
        CustomMinimumSize = new Vector2(160, 160);
        UnsnappedSize = CustomMinimumSize;
        UnsnappedPosition = PositionOffset;

        // Enable/disable frame features.
        AutoshrinkEnabled = false;
        TintColorEnabled = true;

        // Set defaults.
        Title = "New Frame";
        TintColor = new(0.123f, 0.123f, 0.123f);

        // Add selection control.
        SelectionControl = new();
        AddChild(SelectionControl);
        SelectionControl.GuiInput += OnSelectionControlGuiInput;
    }

    /* Public methods. */
    public override string ToString()
    {
        return $"Frame (element index {GetIndex()}): \"{Title}\"";
    }

    public void MoveTo(Vector2 position)
    {
        Vector2 oldPosition = PositionOffset;

        // Move.
        UnsnappedPosition = position;
        Snapper.SnapToGrid(this);

        // Also move contained elements.
        Vector2 visualDelta = PositionOffset - oldPosition;
        foreach (IGraphElement element in Elements)
        {
            element.MoveTo(element.UnsnappedPosition + visualDelta);
        }
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
        // We cannot add ourselves or our direct parent.
        if (element == this || element == Frame)
            return;

        // Ignore redundant additions.
        if (element.Frame == this)
            return;

        if (element.Frame != null)
        {
            throw new Exception($"Tried to add element '{element}' to frame '{this}', but the element was already on frame "
                + $"'{element.Frame}'. Make sure to first remove it!");
        }

        // Add element to new frame.
        Elements.Add(element);
        ElementOffsets.Add(element.PositionOffset - PositionOffset);
        element.Frame = this;
    }

    /// <summary>
    /// Remove an element from this frame.
    /// </summary>
    public void RemoveElement(IGraphElement element)
    {
        int index = Elements.IndexOf(element);
        if (index == -1)
        {
            throw new ArgumentException($"The element '{element}' was not on frame '{this}' and could not be removed.",
                nameof(element));
        }

        ElementOffsets.RemoveAt(index);
        Elements.RemoveAt(index);
        element.Frame = null;
    }

    /// <summary>
    /// Update the frame's size and position to have the smallest fit around its elements.
    /// </summary>
    public void FitAroundElements()
    {
        if (Elements.Count == 0)
        {
            UnsnappedSize = Vector2.Zero;
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
            float elementMinX = Elements[i].UnsnappedPosition.X;
            if (elementMinX < minX)
                minX = elementMinX;

            float elementMinY = Elements[i].UnsnappedPosition.Y;
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
        UnsnappedPosition = new(minX, minY);
        UnsnappedSize = new(maxX - minX, maxY - minY);
        Snapper.SnapToGrid(this);

        // Store member offsets.
        ElementOffsets.Clear();
        foreach (IGraphElement element in Elements)
        {
            ElementOffsets.Add(element.PositionOffset - PositionOffset);
        }

        // Also update parent frame.
        if (Frame != null)
            Frame.FitAroundElements();
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

        // Make sure the selection control covers the whole frame.
        SelectionControl.GlobalPosition = GlobalPosition;
        SelectionControl.Size = Size;

        // Make sure we're always after our parent frame in the node order (otherwise the we will
        // draw behind our parent frame).
        if (Frame != null && GetIndex() < Frame.GetIndex())
        {
            GetParent().MoveChild(this, Frame.GetIndex() + 1);
        }

        // Snap to grid.
        Snapper.SnapToGrid(this);
    }

    /* Private methods. */
    private void OnSelectionControlGuiInput(InputEvent @event)
    {
        // Suppress built-in drag & selection.
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                TryRedirectClick(mouseButton.GlobalPosition);
                AcceptEvent();
            }
            else if (!mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left && IsClicked)
            {
                IsClicked = false;
                MouseReleased?.Invoke(this);
                AcceptEvent();
                return;
            }
        }
        else if (@event is InputEventMouseMotion)
        {
            if (IsClicked)
            {
                MouseDragged?.Invoke(this);
                AcceptEvent();
                return;
            }
        }

        // Otherwise, let normal controls still work.
        base._GuiInput(@event);
    }

    private bool TryRedirectClick(Vector2 clickPosition)
    {
        if (GetGlobalRect().HasPoint(clickPosition))
        {
            // Try to redirect to child frames.
            foreach (IGraphElement element in Elements)
            {
                if (element is GraphFrame frame && frame.TryRedirectClick(clickPosition))
                    return true;
            }

            // If we could not, click ourselves.
            IsClicked = true;
            MouseClicked?.Invoke(this);
            return true;
        }

        return false;
    }
}
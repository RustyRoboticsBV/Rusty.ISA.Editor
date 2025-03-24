using Godot;
using Rusty.ISA.ProgramEditor.Compiler;
using System;
using System.Collections.Generic;

namespace Rusty.ISA.ProgramEditor
{
    /// <summary>
    /// A ISA graph frame.
    /// </summary>
    public partial class GraphFrame : Godot.GraphFrame, IGraphElement
    {
        /* Public properties. */
        public ProgramGraphEdit GraphEdit { get; private set; }
        public InstructionSet InstructionSet => GraphEdit.InstructionSet;
        public InstructionDefinition Definition => InstructionSet[BuiltIn.FrameOpcode];

        public FrameInspector Inspector { get; private set; }
        public GraphFrame Frame { get; set; }

        Inspector IGraphElement.Inspector => Inspector;

        public new bool IsSelected => base.Selected;

        /* Private properties. */
        private VBoxContainer InspectorWindow => GraphEdit.InspectorWindow;

        private List<IGraphElement> Elements { get; } = new();
        private Vector2 LastOffset { get; set; }
        private List<Vector2> ElementOffsets { get; } = new();

        /* Public events. */
        public new event Action<IGraphElement> Selected;
        public event Action<IGraphElement> Deselected;
        public new event Action<IGraphElement> Dragged;
        public event Action<IGraphElement> Deleted;

        /* Constructors. */
        public GraphFrame(ProgramGraphEdit graphEdit, Vector2 positionOffset)
        {
            GraphEdit = graphEdit;
            PositionOffset = positionOffset;
            LastOffset = positionOffset;

            AutoshrinkEnabled = false;
            TintColorEnabled = true;
            CustomMinimumSize = Vector2.One;
            Size = Vector2.One * 64f;

            Inspector = new(InstructionSet);

            NodeSelected += OnNodeSelected;
            NodeDeselected += OnNodeDeselected;
            base.Dragged += OnDragged;

            UpdateArguments();
        }

        /* Public methods. */
        public bool IsNestedIn(GraphFrame frame)
        {
            return Frame == frame || Frame != null && Frame.IsNestedIn(frame);
        }

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
            element.Deleted += OnElementDeleted;

            // Alter position & size.
            UpdateSizePosition();
        }

        public void RemoveElement(IGraphElement element)
        {
            // Remove.
            Elements.Remove(element);
            element.Frame = null;
            element.Dragged -= OnElementDragged;

            // Alter position & size.
            UpdateSizePosition();
        }

        public void Delete()
        {
            // Move all elements to the parent frame (if there is one).
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Frame = null;
                if (Frame != null)
                    Frame.AddElement(Elements[i]);
            }

            // Remove from parent object.
            Node parent = GetParent();
            if (parent != null)
                parent.RemoveChild(this);

            // Call delete event.
            Deleted?.Invoke(this);
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            // Copy title label from the inspector's value.
            UpdateArguments();

            // If we move the frame, also move members with it.
            if (LastOffset != PositionOffset)
            {
                for (int i = 0; i < Elements.Count; i++)
                {
                    Elements[i].PositionOffset = PositionOffset + ElementOffsets[i];
                }
                LastOffset = PositionOffset;
            }
        }

        /* Private methods. */
        private void UpdateSizePosition()
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
                Frame.UpdateSizePosition();
        }

        private void UpdateArguments()
        {
            Title = Inspector.TitleText;

            Color color = Inspector.Color * Definition.EditorNode.MainColor;
            color.A *= IsSelected ? 1f : 0.5f;
            TintColor = color;
        }


        private void OnNodeSelected()
        {
            Selected?.Invoke(this);
        }

        private void OnNodeDeselected()
        {
            Deselected?.Invoke(this);
        }

        private void OnDragged(Vector2 from, Vector2 to)
        {
            Dragged?.Invoke(this);
            LastOffset = to;
        }

        private void OnElementDragged(IGraphElement element)
        {
            if (!IsSelected)
                UpdateSizePosition();
        }

        private void OnElementDeleted(IGraphElement element)
        {
            RemoveElement(element);
        }
    }
}
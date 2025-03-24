using Godot;
using System;

namespace Rusty.ISA.ProgramEditor
{
    /// <summary>
    /// A graph edit node representing a collection of instructions.
    /// </summary>
    public abstract partial class GraphNode : Godot.GraphNode, IGraphElement
    {
        /* Public properties. */
        public ProgramGraphEdit GraphEdit { get; private set; }
        public GraphFrame Frame { get; set; }
        public Inspector Inspector { get; protected set; }

        public InstructionSet InstructionSet => GraphEdit.InstructionSet;
        public InstructionDefinition Definition { get; private set; }

        public new bool IsSelected => base.Selected;

        /* Protected properties. */
        protected VBoxContainer InspectorWindow => GraphEdit.InspectorWindow;
        protected HBoxContainer TitleContainer { get; private set; }

        /* Public events. */
        public new event Action<IGraphElement> Selected;
        public event Action<IGraphElement> Deselected;
        public new event Action<IGraphElement> Dragged;
        public event Action<IGraphElement> Deleted;

        /* Constructors. */
        public GraphNode(ProgramGraphEdit graphEdit, InstructionDefinition definition)
        {
            GraphEdit = graphEdit;
            Definition = definition;
            TitleContainer = GetTitlebarHBox();

            NodeSelected += OnNodeSelected;
            NodeDeselected += OnNodeDeselected;
            base.Dragged += OnDragged;
        }

        /* Public methods. */
        public void Delete()
        {
            Node parent = GetParent();
            if (parent != null)
                parent.RemoveChild(this);
            Deleted?.Invoke(this);
        }

        /* Protected methods. */
        protected virtual void OnNodeSelected()
        {
            Selected?.Invoke(this);
        }

        protected virtual void OnNodeDeselected()
        {
            Deselected?.Invoke(this);
        }

        protected virtual void OnDragged(Vector2 from, Vector2 to)
        {
            Dragged?.Invoke(this);
        }
    }
}
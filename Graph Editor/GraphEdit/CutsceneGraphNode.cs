using Godot;
using System;
using Rusty.Cutscenes;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A graph edit node representing a collection of cutscene instructions.
    /// </summary>
    [GlobalClass]
    public abstract partial class CutsceneGraphNode : GraphNode, IGraphElement
    {
        /* Public properties. */
        public CutsceneGraphEdit GraphEdit { get; private set; }
        public CutsceneGraphFrame Frame { get; set; }
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
        public CutsceneGraphNode(CutsceneGraphEdit graphEdit, InstructionDefinition definition)
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
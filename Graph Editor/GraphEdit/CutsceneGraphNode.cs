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
        public CutsceneGraphFrame Frame
        {
            get
            {
                Node parent = GetParent();
                if (parent is CutsceneGraphFrame frame)
                    return frame;
                return null;
            }
        }
        public Inspector Inspector { get; protected set; }

        public InstructionSet InstructionSet => GraphEdit.InstructionSet;
        public InstructionDefinition Definition { get; private set; }

        /* Protected properties. */
        protected VBoxContainer InspectorWindow => GraphEdit.InspectorWindow;
        protected HBoxContainer TitleContainer { get; private set; }

        /* Public events. */
        public event Action<IGraphElement> OnSelected;
        public event Action<IGraphElement> OnDeselected;

        /* Constructors. */
        public CutsceneGraphNode(CutsceneGraphEdit graphEdit, InstructionDefinition definition)
        {
            GraphEdit = graphEdit;
            Definition = definition;
            TitleContainer = GetTitlebarHBox();

            NodeSelected += OnNodeSelected;
            NodeDeselected += OnNodeDeselected;
        }

        /* Protected methods. */
        protected virtual void OnNodeSelected()
        {
            if (this is IGraphElement element)
                OnSelected?.Invoke(element);
        }

        protected virtual void OnNodeDeselected()
        {
            if (this is IGraphElement element)
                OnDeselected?.Invoke(element);
        }
    }
}
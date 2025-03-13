using Godot;
using Rusty.Cutscenes;
using Rusty.CutsceneEditor.Compiler;
using System;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A cutscene graph frame.
    /// </summary>
    public partial class CutsceneGraphFrame : GraphFrame, IGraphElement
    {
        /* Public properties. */
        public CutsceneGraphEdit GraphEdit { get; private set; }
        public InstructionSet InstructionSet => GraphEdit.InstructionSet;
        public InstructionDefinition Definition => InstructionSet[BuiltIn.FrameOpcode];

        public FrameInspector Inspector { get; private set; }
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

        Inspector IGraphElement.Inspector => Inspector;

        /* Private properties. */
        private VBoxContainer InspectorWindow => GraphEdit.InspectorWindow;

        /* Public events. */
        public event Action<IGraphElement> OnSelected;
        public event Action<IGraphElement> OnDeselected;

        /* Constructors. */
        public CutsceneGraphFrame(CutsceneGraphEdit graphEdit, Vector2 positionOffset)
        {
            GraphEdit = graphEdit;
            PositionOffset = positionOffset;

            AutoshrinkEnabled = false;
            CustomMinimumSize = Vector2.One;
            Size = Vector2.One * 64f;
            Title = "Frame";

            Inspector = new(InstructionSet);

            NodeSelected += OnNodeSelected;
            NodeDeselected += OnNodeDeselected;
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            if (Selected)
                Title = Inspector.TitleText;
        }

        /* Private methods. */
        private void OnNodeSelected()
        {
            OnSelected?.Invoke(this);
        }

        private void OnNodeDeselected()
        {
            OnDeselected?.Invoke(this);
        }
    }
}
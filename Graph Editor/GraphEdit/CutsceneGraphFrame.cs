using Godot;
using Rusty.Cutscenes;
using Rusty.CutsceneEditor.Compiler;

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

        public Inspector Inspector { get; private set; }
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

        /* Private methods. */
        private VBoxContainer InspectorWindow => GraphEdit.InspectorWindow;

        /* Constructors. */
        public CutsceneGraphFrame(CutsceneGraphEdit graphEdit, Vector2 positionOffset)
        {
            GraphEdit = graphEdit;
            PositionOffset = positionOffset;

            AutoshrinkEnabled = false;
            CustomMinimumSize = Vector2.One;
            Size = Vector2.One * 64f;
            Title = "Frame";

            Inspector = new FrameInspector(InstructionSet);
        }
    }
}
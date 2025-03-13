using Rusty.CutsceneEditor.Compiler;
using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A cutscene resource inspector.
    /// </summary>
    public partial class FrameInspector : Inspector
    {
        /* Public properties. */
        /// <summary>
        /// The current value of the comment.
        /// </summary>
        public string TitleText
        {
            get => Field.Value;
            set => Field.Value = value;
        }
        /// <summary>
        /// The comment instruction definition.
        /// </summary>
        public InstructionDefinition Definition
        {
            get => Resource as InstructionDefinition;
            set => Resource = value;
        }

        /* Private properties. */
        private LineField Field { get; set; }

        /* Constructors. */
        public FrameInspector() : base() { }

        public FrameInspector(InstructionSet instructionSet) : base(instructionSet, instructionSet[BuiltIn.FrameOpcode]) { }

        public FrameInspector(FrameInspector other) : base(other) { }

        /* Public methods. */
        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is FrameInspector otherInspector)
            {
                TitleText = otherInspector.TitleText;
                return true;
            }
            else
                return false;
        }

        /* Protected methods. */
        /// <summary>
        /// Inspector initialization method.
        /// </summary>
        protected override void Init()
        {
            base.Init();

            TextParameter parameter = Definition.Parameters[Definition.GetParameterIndex(BuiltIn.FrameTitle)] as TextParameter;
            Field = new LineField()
            {
                Value = parameter.DefaultValue
            };
            Add(Field);
        }
    }
}
using Godot;
using Rusty.ISA.Editor.Programs.Compiler;
using Rusty.ISA;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Programs
{
    /// <summary>
    /// A ISA resource inspector.
    /// </summary>
    public partial class FrameInspector : Inspector
    {
        /* Public properties. */
        /// <summary>
        /// The comment instruction definition.
        /// </summary>
        public InstructionDefinition Definition
        {
            get => Resource as InstructionDefinition;
            set => Resource = value;
        }

        /// <summary>
        /// The current title text of the frame.
        /// </summary>
        public string TitleText
        {
            get => TitleField.Value;
            set => TitleField.Value = value;
        }
        /// <summary>
        /// The current color tint of the frame.
        /// </summary>
        public Color Color
        {
            get => ColorField.Value;
            set => ColorField.Value = value;
        }

        /* Private properties. */
        private LineField TitleField { get; set; }
        private ColorField ColorField { get; set; }

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
                Color = otherInspector.Color;
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

            TextlineParameter titleParameter = Definition.Parameters[Definition.GetParameterIndex(BuiltIn.FrameTitle)] as TextlineParameter;
            ColorParameter colorParameter = Definition.Parameters[Definition.GetParameterIndex(BuiltIn.FrameColor)] as ColorParameter;
            Add(new LabeledIcon()
            {
                Name = "Header",
                LabelText = Definition.DisplayName,
                Value = Definition.Icon
            });
            TitleField = new()
            {
                Name = "Title",
                LabelText = "Title",
                Value = titleParameter.DefaultValue
            };
            Add(TitleField);
            ColorField = new()
            {
                Name = "Color",
                LabelText = "Color",
                Value = colorParameter.DefaultValue
            };
            Add(ColorField);
        }
    }
}
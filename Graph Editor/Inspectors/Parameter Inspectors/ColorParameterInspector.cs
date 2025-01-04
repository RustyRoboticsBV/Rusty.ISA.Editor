using Godot;
using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A color parameter inspector.
    /// </summary>
    public partial class ColorParameterInspector : ParameterInspector
    {
        /* Public properties. */
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public new ColorParameter Definition
        {
            get => base.Definition as ColorParameter;
            set => base.Definition = value;
        }
        /// <summary>
        /// The value of the stored parameter.
        /// </summary>
        public Color Value
        {
            get => ColorField.Value;
            set => ColorField.Value = value;
        }
        public override object ValueObj
        {
            get => Value;
            set => Value = (Color)ValueObj;
        }

        /* Private properties. */
        private ColorField ColorField { get; set; }

        /* Constructors. */
        public ColorParameterInspector() : base() { }

        public ColorParameterInspector(InstructionSet instructionSet, ColorParameter parameter)
            : base(instructionSet, parameter)
        {
            ColorField.Value = parameter.DefaultValue;
        }

        public ColorParameterInspector(ColorParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new ColorParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is ColorParameterInspector otherInspector)
            {
                ColorField = GetAt(0) as ColorField;
                Value = otherInspector.Value;
                return true;
            }
            return false;
        }

        /* Protected methods. */
        protected override void Init()
        {
            // Base parameter inspector init.
            base.Init();

            // Set name.
            Name = $"ColorParameter ({Definition.Id})";

            // Add Color field.
            ColorField = new();
            ColorField.LabelText = Definition.DisplayName;
            Add(ColorField);
        }
    }
}
using Godot;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Programs
{
    /// <summary>
    /// A float parameter inspector.
    /// </summary>
    [GlobalClass]
    public partial class IntSliderParameterInspector : ParameterInspector
    {
        /* Public properties. */
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public new IntSliderParameter Definition
        {
            get => base.Definition as IntSliderParameter;
            set => base.Definition = value;
        }
        /// <summary>
        /// The value of the stored parameter.
        /// </summary>
        public int Value
        {
            get => IntSliderField.Value;
            set => IntSliderField.Value = value;
        }
        public override object ValueObj
        {
            get => Value;
            set => Value = (int)value;
        }

        /* Private properties. */
        private IntSliderField IntSliderField { get; set; }

        /* Constructors. */
        public IntSliderParameterInspector(InstructionInspector root, IntSliderParameter parameter)
            : base(root, parameter)
        {
            IntSliderField.Value = parameter.DefaultValue;
            IntSliderField.MinValue = parameter.MinValue;
            IntSliderField.MaxValue = parameter.MaxValue;
        }

        public IntSliderParameterInspector(IntSliderParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new IntSliderParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is IntSliderParameterInspector otherInspector)
            {
                IntSliderField = GetAt(0) as IntSliderField;
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
            Name = $"IntSliderParameter ({Definition.ID})";

            // Add int slider field.
            IntSliderField = new();
            IntSliderField.LabelText = Definition.DisplayName;
            Add(IntSliderField);
        }
    }
}
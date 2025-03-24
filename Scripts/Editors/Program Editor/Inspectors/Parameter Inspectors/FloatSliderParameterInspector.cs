using Godot;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor
{
    /// <summary>
    /// A float parameter inspector.
    /// </summary>
    [GlobalClass]
    public partial class FloatSliderParameterInspector : ParameterInspector
    {
        /* Public properties. */
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public new FloatSliderParameter Definition
        {
            get => base.Definition as FloatSliderParameter;
            set => base.Definition = value;
        }
        /// <summary>
        /// The value of the stored parameter.
        /// </summary>
        public float Value
        {
            get => FloatSliderField.Value;
            set => FloatSliderField.Value = value;
        }
        public override object ValueObj
        {
            get => Value;
            set => Value = (float)value;
        }

        /* Private properties. */
        private FloatSliderField FloatSliderField { get; set; }

        /* Constructors. */
        public FloatSliderParameterInspector(InstructionInspector root, FloatSliderParameter parameter)
            : base(root, parameter)
        {
            FloatSliderField.Value = parameter.DefaultValue;
            FloatSliderField.MinValue = parameter.MinValue;
            FloatSliderField.MaxValue = parameter.MaxValue;
        }

        public FloatSliderParameterInspector(FloatSliderParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new FloatSliderParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is FloatSliderParameterInspector otherInspector)
            {
                FloatSliderField = GetAt(0) as FloatSliderField;
                Value = otherInspector.Value;
                return true;
            }
            return false;
        }

        public override void SetValue(string str)
        {
            try
            {
                Value = float.Parse(str);
            }
            catch
            {
                Value = 0f;
            }
        }

        /* Protected methods. */
        protected override void Init()
        {
            // Base parameter inspector init.
            base.Init();

            // Set name.
            Name = $"FloatSliderParameter ({Definition.ID})";

            // Add float slider field.
            FloatSliderField = new();
            FloatSliderField.LabelText = Definition.DisplayName;
            Add(FloatSliderField);
        }
    }
}
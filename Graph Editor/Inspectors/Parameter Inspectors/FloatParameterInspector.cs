using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A float parameter inspector.
    /// </summary>
    public partial class FloatParameterInspector : ParameterInspector
    {
        /* Public properties. */
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public new FloatParameter Definition
        {
            get => base.Definition as FloatParameter;
            set => base.Definition = value;
        }
        /// <summary>
        /// The value of the stored parameter.
        /// </summary>
        public float Value
        {
            get => FloatField.Value;
            set => FloatField.Value = value;
        }
        public override object ValueObj
        {
            get => Value;
            set => Value = (float)value;
        }

        /* Private properties. */
        private FloatField FloatField { get; set; }

        /* Constructors. */
        public FloatParameterInspector() : base() { }

        public FloatParameterInspector(InstructionSet instructionSet, FloatParameter parameter)
            : base(instructionSet, parameter)
        {
            FloatField.Value = parameter.DefaultValue;
        }

        public FloatParameterInspector(FloatParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new FloatParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is FloatParameterInspector otherInspector)
            {
                FloatField = GetAt(0) as FloatField;
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
            Name = $"FloatParameter ({Definition.ID})";

            // Add float field.
            FloatField = new();
            FloatField.LabelText = Definition.DisplayName;
            Add(FloatField);
        }
    }
}
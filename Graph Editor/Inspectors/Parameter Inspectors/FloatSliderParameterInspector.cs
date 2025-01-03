using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A float parameter inspector.
    /// </summary>
    public partial class FloatSliderParameterInspector : ParameterInspector
    {
        /* Public properties. */
        public float Value
        {
            get => FloatSliderField.Value;
            set => FloatSliderField.Value = value;
        }

        /* Private properties. */
        private FloatSliderField FloatSliderField { get; set; }

        /* Constructors. */
        public FloatSliderParameterInspector() : base() { }

        public FloatSliderParameterInspector(InstructionSet instructionSet, FloatSliderParameter parameter)
            : base(instructionSet, parameter)
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
                Value = otherInspector.Value;
                return true;
            }
            return false;
        }

        /* Protected methods. */
        protected override void Init(ParameterDefinition resource)
        {
            // Base parameter inspector init.
            base.Init(resource);

            // Set name.
            Name = $"FloatSliderParameter ({resource.Id})";

            // Add float slider field.
            FloatSliderField = new();
            FloatSliderField.LabelText = resource.DisplayName;
            Add(FloatSliderField);
        }
    }
}
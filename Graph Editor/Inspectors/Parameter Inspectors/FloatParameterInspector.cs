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
        public float Value
        {
            get => FloatField.Value;
            set => FloatField.Value = value;
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
            Name = $"FloatParameter ({resource.Id})";

            // Add float field.
            FloatField = new();
            FloatField.LabelText = resource.DisplayName;
            Add(FloatField);
        }
    }
}
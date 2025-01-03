using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A float parameter inspector.
    /// </summary>
    public partial class IntSliderParameterInspector : ParameterInspector
    {
        /* Public properties. */
        public int Value
        {
            get => IntSliderField.Value;
            set => IntSliderField.Value = value;
        }

        /* Private properties. */
        private IntSliderField IntSliderField { get; set; }

        /* Constructors. */
        public IntSliderParameterInspector() : base() { }

        public IntSliderParameterInspector(InstructionSet instructionSet, IntSliderParameter parameter)
            : base(instructionSet, parameter)
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
            Name = $"IntSliderParameter ({resource.Id})";

            // Add int slider field.
            IntSliderField = new();
            IntSliderField.LabelText = resource.DisplayName;
            Add(IntSliderField);
        }
    }
}
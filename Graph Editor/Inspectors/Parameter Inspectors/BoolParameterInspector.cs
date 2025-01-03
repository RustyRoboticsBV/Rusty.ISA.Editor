using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A bool parameter inspector.
    /// </summary>
    public partial class BoolParameterInspector : ParameterInspector
    {
        /* Public properties. */
        public bool Value
        {
            get => CheckBox.Value;
            set => CheckBox.Value = value;
        }

        /* Private properties. */
        private CheckBoxField CheckBox { get; set; }

        /* Constructors. */
        public BoolParameterInspector() : base() { }

        public BoolParameterInspector(InstructionSet instructionSet, BoolParameter parameter)
            : base(instructionSet, parameter)
        {
            CheckBox.Value = parameter.DefaultValue;
        }

        public BoolParameterInspector(BoolParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new BoolParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is BoolParameterInspector otherInspector)
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
            Name = $"BoolParameter ({resource.Id})";

            // Add check box field.
            CheckBox = new();
            CheckBox.LabelText = resource.DisplayName;
            Add(CheckBox);
        }
    }
}
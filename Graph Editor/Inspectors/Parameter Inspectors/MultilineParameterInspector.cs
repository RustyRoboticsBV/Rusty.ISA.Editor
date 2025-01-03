using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A string parameter inspector.
    /// </summary>
    public partial class MultilineParameterInspector : ParameterInspector
    {
        /* Public properties. */
        public string Value
        {
            get => MultilineField.Value;
            set => MultilineField.Value = value;
        }

        /* Private properties. */
        private MultilineField MultilineField { get; set; }

        /* Constructors. */
        public MultilineParameterInspector() : base() { }

        public MultilineParameterInspector(InstructionSet instructionSet, MultilineParameter parameter)
            : base(instructionSet, parameter)
        {
            MultilineField.Value = parameter.DefaultValue;
        }

        public MultilineParameterInspector(MultilineParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new MultilineParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is MultilineParameterInspector otherInspector)
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
            Name = $"MultilineParameter ({resource.Id})";

            // Add multiline field.
            MultilineField = new();
            MultilineField.LabelText = resource.DisplayName;
            MultilineField.Height = 128f;
            Add(MultilineField);
        }
    }
}
using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A line parameter inspector.
    /// </summary>
    public partial class LineParameterInspector : ParameterInspector
    {
        /* Public properties. */
        public string Value
        {
            get => LineField.Value;
            set => LineField.Value = value;
        }

        /* Private properties. */
        private LineField LineField { get; set; }

        /* Constructors. */
        public LineParameterInspector() : base() { }

        public LineParameterInspector(InstructionSet instructionSet, LineParameter parameter)
            : base(instructionSet, parameter)
        {
            LineField.Value = parameter.DefaultValue;
        }

        public LineParameterInspector(LineParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new LineParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is LineParameterInspector otherInspector)
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
            Name = $"LineParameter ({resource.Id})";

            // Add line field.
            LineField = new();
            LineField.LabelText = resource.DisplayName;
            Add(LineField);
        }
    }
}
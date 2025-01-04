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
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public new LineParameter Definition
        {
            get => base.Definition as LineParameter;
            set => base.Definition = value;
        }
        /// <summary>
        /// The value of the stored parameter.
        /// </summary>
        public string Value
        {
            get => LineField.Value;
            set => LineField.Value = value;
        }
        public override object ValueObj
        {
            get => Value;
            set => Value = (string)ValueObj;
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
                LineField = GetAt(0) as LineField;
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
            Name = $"LineParameter ({Definition.Id})";

            // Add line field.
            LineField = new();
            LineField.LabelText = Definition.DisplayName;
            Add(LineField);
        }
    }
}
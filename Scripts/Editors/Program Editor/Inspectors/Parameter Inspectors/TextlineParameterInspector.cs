using Godot;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Programs
{
    /// <summary>
    /// A line parameter inspector.
    /// </summary>
    [GlobalClass]
    public partial class TextlineParameterInspector : ParameterInspector
    {
        /* Public properties. */
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public new TextlineParameter Definition
        {
            get => base.Definition as TextlineParameter;
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
            set => Value = (string)value;
        }

        /* Private properties. */
        private LineField LineField { get; set; }

        /* Constructors. */
        public TextlineParameterInspector(InstructionInspector root, TextlineParameter parameter)
            : base(root, parameter)
        {
            LineField.Value = parameter.DefaultValue;
        }

        public TextlineParameterInspector(TextlineParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new TextlineParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is TextlineParameterInspector otherInspector)
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
            Name = $"TextlineParameter ({Definition.ID})";

            // Add line field.
            LineField = new();
            LineField.LabelText = Definition.DisplayName;
            Add(LineField);
        }
    }
}
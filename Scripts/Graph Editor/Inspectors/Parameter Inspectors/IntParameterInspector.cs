using Rusty.ISA;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor
{
    /// <summary>
    /// An int parameter inspector.
    /// </summary>
    public partial class IntParameterInspector : ParameterInspector
    {
        /* Public properties. */
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public new IntParameter Definition
        {
            get => base.Definition as IntParameter;
            set => base.Definition = value;
        }
        /// <summary>
        /// The value of the stored parameter.
        /// </summary>
        public int Value
        {
            get => IntField.Value;
            set => IntField.Value = value;
        }
        public override object ValueObj
        {
            get => Value;
            set => Value = (int)value;
        }

        /* Private properties. */
        private IntField IntField { get; set; }

        /* Constructors. */
        public IntParameterInspector() : base() { }

        public IntParameterInspector(InstructionSet instructionSet, IntParameter parameter)
            : base(instructionSet, parameter)
        {
            IntField.Value = parameter.DefaultValue;
        }

        public IntParameterInspector(IntParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new IntParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is IntParameterInspector otherInspector)
            {
                IntField = GetAt(0) as IntField;
                Value = otherInspector.Value;
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
            Name = $"IntParameter ({Definition.ID})";

            // Add int field.
            IntField = new();
            IntField.LabelText = Definition.DisplayName;
            Add(IntField);
        }
    }
}
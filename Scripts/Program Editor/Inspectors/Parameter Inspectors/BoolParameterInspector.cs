using Godot;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor
{
    /// <summary>
    /// A bool parameter inspector.
    /// </summary>
    [GlobalClass]
    public partial class BoolParameterInspector : ParameterInspector
    {
        /* Public properties. */
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public new BoolParameter Definition
        {
            get => base.Definition as BoolParameter;
            set => base.Definition = value;
        }
        /// <summary>
        /// The value of the stored parameter.
        /// </summary>
        public bool Value
        {
            get => CheckBox.Value;
            set => CheckBox.Value = value;
        }
        public override object ValueObj
        {
            get => Value;
            set => Value = (bool)value;
        }

        /* Private properties. */
        private CheckBoxField CheckBox { get; set; }

        /* Constructors. */
        public BoolParameterInspector(InstructionInspector root, BoolParameter parameter)
            : base(root, parameter)
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
                CheckBox = GetAt(0) as CheckBoxField;
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
            Name = $"BoolParameter ({Definition.ID})";

            // Add check box field.
            CheckBox = new();
            CheckBox.LabelText = Definition.DisplayName;
            Add(CheckBox);
        }
    }
}
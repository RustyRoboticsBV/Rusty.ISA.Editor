using Godot;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Programs
{
    /// <summary>
    /// A line parameter inspector.
    /// </summary>
    [GlobalClass]
    public partial class CharParameterInspector : ParameterInspector
    {
        /* Public properties. */
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public new CharParameter Definition
        {
            get => base.Definition as CharParameter;
            set => base.Definition = value;
        }
        /// <summary>
        /// The value of the stored parameter.
        /// </summary>
        public char Value
        {
            get => CharField.Value;
            set => CharField.Value = value;
        }
        public override object ValueObj
        {
            get => Value;
            set => Value = (char)value;
        }

        /* Private properties. */
        private CharField CharField { get; set; }

        /* Constructors. */
        public CharParameterInspector(InstructionInspector root, CharParameter parameter)
            : base(root, parameter)
        {
            CharField.Value = parameter.DefaultValue;
        }

        public CharParameterInspector(CharParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new CharParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is CharParameterInspector otherInspector)
            {
                CharField = GetAt(0) as CharField;
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
            Name = $"CharParameter ({Definition.ID})";

            // Add line field.
            CharField = new();
            CharField.LabelText = Definition.DisplayName;
            Add(CharField);
        }
    }
}
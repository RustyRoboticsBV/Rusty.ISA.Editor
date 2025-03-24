using Godot;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Programs
{
    /// <summary>
    /// A output parameter inspector. It's a dummy inspector that is instantly hidden.
    /// </summary>
    [GlobalClass]
    public partial class OutputParameterInspector : ParameterInspector
    {
        /* Public properties. */
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public new OutputParameter Definition
        {
            get => base.Definition as OutputParameter;
            set => base.Definition = value;
        }
        public override object ValueObj
        {
            get => null;
            set { }
        }

        /* Constructors. */
        public OutputParameterInspector(InstructionInspector root, OutputParameter parameter)
            : base(root, parameter) { }

        public OutputParameterInspector(OutputParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new OutputParameterInspector(this);
        }

        /* Protected methods. */
        protected override void Init()
        {
            // Base parameter inspector init.
            base.Init();

            // Set name.
            Name = $"OutputParameter ({Definition.ID})";

            // Hide this inspector.
            Hide();
        }
    }
}
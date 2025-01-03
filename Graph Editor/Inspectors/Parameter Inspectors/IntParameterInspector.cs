using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// An int parameter inspector.
    /// </summary>
    public partial class IntParameterInspector : ParameterInspector
    {
        /* Public properties. */
        public int Value
        {
            get => IntField.Value;
            set => IntField.Value = value;
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
            Name = $"IntParameter ({resource.Id})";

            // Add int field.
            IntField = new();
            IntField.LabelText = resource.DisplayName;
            Add(IntField);
        }
    }
}
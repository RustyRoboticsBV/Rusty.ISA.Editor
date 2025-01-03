using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A output parameter inspector. It's a dummy inspector that is instantly hidden.
    /// </summary>
    public partial class OutputParameterInspector : ParameterInspector
    {
        /* Constructors. */
        public OutputParameterInspector() : base() { }

        public OutputParameterInspector(InstructionSet instructionSet, OutputParameter parameter)
            : base(instructionSet, parameter) { }

        public OutputParameterInspector(OutputParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new OutputParameterInspector(this);
        }

        /* Protected methods. */
        protected override void Init(ParameterDefinition resource)
        {
            // Base parameter inspector init.
            base.Init(resource);

            // Set name.
            Name = $"OutputParameter ({resource.Id})";

            // Hide this inspector.
            Hide();
        }
    }
}
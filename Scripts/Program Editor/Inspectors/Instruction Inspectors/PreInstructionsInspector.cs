using Rusty.ISA;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor
{
    public partial class PreInstructionsInspector : SecondaryInstructionsInspector
    {
        /* Constructors. */
        public PreInstructionsInspector() : base() { }

        public PreInstructionsInspector(InstructionSet instructionSet, InstructionDefinition definition)
            : base(instructionSet, definition) { }

        public PreInstructionsInspector(PreInstructionsInspector other) : base()
        {
            CopyStateFrom(other);
        }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new PreInstructionsInspector(this);
        }

        /* Protected methods. */
        protected override void Init()
        {
            base.Init();

            Name = "Pre Instructions";
        }
    }
}
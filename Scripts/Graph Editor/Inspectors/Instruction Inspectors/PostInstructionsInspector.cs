using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    public partial class PostInstructionsInspector : SecondaryInstructionsInspector
    {
        /* Constructors. */
        public PostInstructionsInspector() : base() { }

        public PostInstructionsInspector(InstructionSet instructionSet, InstructionDefinition definition)
            : base(instructionSet, definition) { }

        public PostInstructionsInspector(PostInstructionsInspector other) : this()
        {
            CopyStateFrom(other);
        }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new PostInstructionsInspector(this);
        }

        /* Protected methods. */
        protected override void Init()
        {
            base.Init();

            Name = "Post Instructions";
        }
    }
}
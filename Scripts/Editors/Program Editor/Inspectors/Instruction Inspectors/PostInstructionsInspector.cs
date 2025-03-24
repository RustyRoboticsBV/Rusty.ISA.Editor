using Rusty.EditorUI;

namespace Rusty.ISA.ProgramEditor
{
    public partial class PostInstructionsInspector : SecondaryInstructionsInspector
    {
        /* Constructors. */
        public PostInstructionsInspector() : base() { }

        public PostInstructionsInspector(InstructionInspector root, InstructionDefinition definition)
            : base(root, definition) { }

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
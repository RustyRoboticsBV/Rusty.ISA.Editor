using Godot;
using Rusty.Cutscenes;
using Rusty.Cutscenes.Editor;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A pre-instruction inspector.
    /// </summary>
    public partial class PreInstructionInspector : CompileRuleInspector
    {
        /* Public methods. */
        /// <summary>
        /// The compile rule visualized by this inspector.
        /// </summary>
        public PreInstruction Rule => Resource as PreInstruction;

        /* Private properties. */
        private InstructionInspector InstructionInspector { get; set; }

        /* Constructors. */
        public PreInstructionInspector() : base() { }

        public PreInstructionInspector(InstructionSet instructionSet, PreInstruction compileRule)
            : base(instructionSet, compileRule) { }

        public PreInstructionInspector(PreInstructionInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new PreInstructionInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other))
            {
                InstructionInspector = GetAt(0) as InstructionInspector;
                return true;
            }
            else
                return false;
        }

        /* Protected methods. */
        protected override void Init(CompileRule resource)
        {
            // Base compile rule inspector init.
            base.Init(resource);

            // Set name.
            Name = $"PreInstruction ({resource.Id})";

            // Add option element.
            InstructionInspector = new(InstructionSet, InstructionSet[Rule.Opcode]);
            InstructionInspector.RemoveTitleAndLabel();
            Add(InstructionInspector);
        }
    }
}
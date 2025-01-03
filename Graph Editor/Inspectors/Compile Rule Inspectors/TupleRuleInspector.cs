using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A tuple rule inspector.
    /// </summary>
    public partial class TupleRuleInspector : CompileRuleInspector
    {
        /* Public methods. */
        /// <summary>
        /// The compile rule visualized by this inspector.
        /// </summary>
        public TupleRule Rule => Resource as TupleRule;

        /* Private properties. */
        private OptionField OptionField { get; set; }

        /* Constructors. */
        public TupleRuleInspector() : base() { }

        public TupleRuleInspector(InstructionSet instructionSet, TupleRule compileRule)
            : base(instructionSet, compileRule) { }

        public TupleRuleInspector(TupleRuleInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new TupleRuleInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other))
            {
                OptionField = GetAt(0) as OptionField;
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
            Name = $"TupleRule ({resource.Id})";

            // Add option element.
            OptionField = new();
            Add(OptionField);
        }
    }
}
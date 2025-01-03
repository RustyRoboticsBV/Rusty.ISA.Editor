using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A choice rule inspector.
    /// </summary>
    public partial class ChoiceRuleInspector : CompileRuleInspector
    {
        /* Public methods. */
        /// <summary>
        /// The compile rule visualized by this inspector.
        /// </summary>
        public ChoiceRule Rule => Resource as ChoiceRule;

        /* Private properties. */
        private OptionField OptionField { get; set; }

        /* Constructors. */
        public ChoiceRuleInspector() : base() { }

        public ChoiceRuleInspector(InstructionSet instructionSet, ChoiceRule compileRule)
            : base(instructionSet, compileRule) { }

        public ChoiceRuleInspector(ChoiceRuleInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new ChoiceRuleInspector(this);
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
            Name = $"ChoiceRule ({resource.Id})";

            // Add option element.
            OptionField = new();
            Add(OptionField);
        }
    }
}
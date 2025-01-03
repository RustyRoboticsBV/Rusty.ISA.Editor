using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A option rule inspector.
    /// </summary>
    public partial class OptionRuleInspector : CompileRuleInspector
    {
        /* Public methods. */
        /// <summary>
        /// The compile rule visualized by this inspector.
        /// </summary>
        public OptionRule Rule => Resource as OptionRule;

        /* Private properties. */
        private CheckBoxField CheckBoxField { get; set; }

        /* Constructors. */
        public OptionRuleInspector() : base() { }

        public OptionRuleInspector(InstructionSet instructionSet, OptionRule compileRule)
            : base(instructionSet, compileRule) { }

        public OptionRuleInspector(OptionRuleInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new OptionRuleInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other))
            {
                CheckBoxField = GetAt(0) as CheckBoxField;
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
            Name = $"OptionRule ({resource.Id})";

            // Add option element.
            CheckBoxField = new();
            Add(CheckBoxField);
        }
    }
}
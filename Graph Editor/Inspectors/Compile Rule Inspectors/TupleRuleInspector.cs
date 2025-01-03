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
        private Inspector[] ChildRules { get; set; }

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
                ChildRules = new Inspector[Count];
                for (int i = 0; i < Count; i++)
                {
                    ChildRules[i] = this[i] as Inspector;
                }
                return true;
            }
            else
                return false;
        }

        public override Inspector[] GetActiveSubInspectors()
        {
            return ChildRules;
        }

        /* Protected methods. */
        protected override void Init()
        {
            // Base compile rule inspector init.
            base.Init();

            // Set name.
            Name = $"TupleRule ({Rule.Id})";

            // Add child rule inspectors.
            ChildRules = new Inspector[Rule.Types.Length];
            for (int i = 0; i < Rule.Types.Length; i++)
            {
                ChildRules[i] = Create(InstructionSet, Rule.Types[i]);
                Add(ChildRules[i]);
            }
        }
    }
}
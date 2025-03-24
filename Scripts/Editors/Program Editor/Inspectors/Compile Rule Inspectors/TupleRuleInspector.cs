using System.Collections.Generic;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor
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
        public new TupleRule Definition => base.Definition as TupleRule;

        /* Private properties. */
        private OptionField OptionField { get; set; }
        private CompileRuleInspector[] ChildRules { get; set; }

        /* Constructors. */
        public TupleRuleInspector(InstructionInspector root, TupleRule compileRule)
            : base(root, compileRule) { }

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
                List<CompileRuleInspector> childRules = new();
                for (int i = 0; i < Count; i++)
                {
                    if (GetAt(i) is CompileRuleInspector inspector)
                        childRules.Add(inspector);
                }
                ChildRules = childRules.ToArray();
                return true;
            }
            else
                return false;
        }

        public override CompileRuleInspector[] GetActiveSubInspectors()
        {
            return ChildRules;
        }

        /* Protected methods. */
        protected override void Init()
        {
            // Base compile rule inspector init.
            base.Init();

            // Set name.
            Name = $"TupleRule ({Definition.ID})";

            // Add label.
            if (Definition.DisplayName != "")
            {
                LabelElement title = new();
                title.Name = "Title";
                title.LabelText = Definition.DisplayName;
                Add(title);
            }

            // Add child rule inspectors.
            ChildRules = new CompileRuleInspector[Definition.Types.Length];
            for (int i = 0; i < Definition.Types.Length; i++)
            {
                ChildRules[i] = Create(Root, Definition.Types[i]);
                Add(ChildRules[i]);
            }
        }
    }
}
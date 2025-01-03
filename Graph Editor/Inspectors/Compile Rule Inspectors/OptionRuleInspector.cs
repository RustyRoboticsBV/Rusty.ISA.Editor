using Rusty.Cutscenes;
using Rusty.EditorUI;
using Rusty.Graphs;

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
        private Inspector ChildRuleInspector { get; set; }

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
                ChildRuleInspector = GetAt(1) as CompileRuleInspector;
                return true;
            }
            else
                return false;
        }

        public override Inspector[] GetActiveSubInspectors()
        {
            if (CheckBoxField.Value)
                return new Inspector[] { ChildRuleInspector };
            else
                return new Inspector[0];
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            base._Process(delta);

            if (ChildRuleInspector != null)
            {
                if (CheckBoxField.Value)
                    ChildRuleInspector.Show();
                else
                    ChildRuleInspector.Hide();
            }
        }

        /* Protected methods. */
        protected override void Init()
        {
            // Base compile rule inspector init.
            base.Init();

            // Set name.
            Name = $"OptionRule ({Rule.Id})";

            // Add option element.
            CheckBoxField = new();
            CheckBoxField.LabelText = Rule.DisplayName;
            CheckBoxField.Value = Rule.StartEnabled;
            Add(CheckBoxField);

            // Add child rule inspector.
            ChildRuleInspector = Create(InstructionSet, Rule.Type);
            Add(ChildRuleInspector);

            if (!Rule.StartEnabled)
                ChildRuleInspector.Hide();
        }
    }
}
using Rusty.EditorUI;

namespace Rusty.ISA.Editor
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
        public new OptionRule Definition => base.Definition as OptionRule;
        /// <summary>
        /// Whether or not this option rule has its check box foldout ticked.
        /// </summary>
        public bool Checked
        {
            get => CheckBoxField.Value;
            set=> CheckBoxField.Value = value;
        }
        public CompileRuleInspector ChildRuleInspector { get; private set; }

        /* Private properties. */
        private CheckBoxField CheckBoxField { get; set; }

        /* Constructors. */
        public OptionRuleInspector(InstructionInspector root, OptionRule compileRule)
            : base(root, compileRule) { }

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

        public override CompileRuleInspector[] GetActiveSubInspectors()
        {
            if (CheckBoxField.Value)
                return new CompileRuleInspector[] { ChildRuleInspector };
            else
                return new CompileRuleInspector[0];
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
            Name = $"OptionRule ({Definition.ID})";

            // Add option element.
            CheckBoxField = new();
            CheckBoxField.LabelText = Definition.DisplayName;
            CheckBoxField.Value = Definition.DefaultEnabled;
            Add(CheckBoxField);

            // Add child rule inspector.
            ChildRuleInspector = Create(Root, Definition.Type);
            ChildRuleInspector.LocalIndentation = 10;
            Add(ChildRuleInspector);

            if (!Definition.DefaultEnabled)
                ChildRuleInspector.Hide();
        }
    }
}
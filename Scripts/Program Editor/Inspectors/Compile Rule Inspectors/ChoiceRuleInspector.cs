using Rusty.EditorUI;

namespace Rusty.ISA.Editor
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
        /// <summary>
        /// The index of the selected choice.
        /// </summary>
        public int Selected
        {
            get => OptionField.Value;
            set => OptionField.Value = value;
        }

        /* Private properties. */
        private OptionField OptionField { get; set; }
        private CompileRuleInspector[] ChildInspectors { get; set; }

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
                // Find option field.
                OptionField = GetAt(0) as OptionField;

                // Find child inspectors.
                ChildInspectors = new CompileRuleInspector[Count - 1];
                for (int i = 1; i < Count; i++)
                {
                    // Find the inspector.
                    CompileRuleInspector inspector = GetAt(i) as CompileRuleInspector;
                    ChildInspectors[i - 1] = inspector;

                    // Hide or show the inspector based on the state of the option field.
                    if (OptionField.Value == i - 1)
                        inspector.Show();
                    else
                        inspector.Hide();
                }

                return true;
            }
            else
                return false;
        }

        public override CompileRuleInspector[] GetActiveSubInspectors()
        {
            try
            {
                return new CompileRuleInspector[] { ChildInspectors[OptionField.Value] };
            }
            catch
            {
                return new CompileRuleInspector[0];
            }
        }

        public CompileRuleInspector GetSelected()
        {
            try
            {
                return ChildInspectors[OptionField.Value];
            }
            catch
            {
                return null;
            }
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            base._Process(delta);

            for (int i = 0; i < ChildInspectors.Length; i++)
            {
                if (OptionField.Value == i)
                    ChildInspectors[i].Show();
                else
                    ChildInspectors[i].Hide();
            }
        }

        /* Protected methods. */
        protected override void Init()
        {
            // Base compile rule inspector init.
            base.Init();

            // Set name.
            Name = $"ChoiceRule ({Rule.ID})";

            // Add option element.
            OptionField = new();
            OptionField.LabelText = Rule.DisplayName;
            OptionField.Value = Rule.DefaultSelected;
            Add(OptionField);

            // Add child rule inspectors.
            ChildInspectors = new CompileRuleInspector[Rule.Types.Length];
            for (int i = 0; i < Rule.Types.Length; i++)
            {
                ChildInspectors[i] = Create(InstructionSet, Rule.Types[i]);
                ChildInspectors[i].LocalIndentation = 10;
                Add(ChildInspectors[i]);
                if (Rule.DefaultSelected != i)
                    ChildInspectors[i].Hide();
            }

            // Set options.
            string[] options = new string[Rule.Types.Length];
            for (int i = 0; i < Rule.Types.Length; i++)
            {
                options[i] = Rule.Types[i].DisplayName;
            }
            OptionField.Options = options;
        }
    }
}
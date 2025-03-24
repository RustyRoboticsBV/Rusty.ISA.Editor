using Rusty.EditorUI;

namespace Rusty.ISA.ProgramEditor
{
    /// <summary>
    /// A list rule inspector.
    /// </summary>
    public partial class ListRuleInspector : CompileRuleInspector
    {
        /* Public methods. */
        /// <summary>
        /// The compile rule visualized by this inspector.
        /// </summary>
        public new ListRule Definition => base.Definition as ListRule;

        /* Private properties. */
        private ListElement ListElement { get; set; }

        /* Constructors. */
        public ListRuleInspector(InstructionInspector root, ListRule compileRule)
            : base(root, compileRule) { }

        public ListRuleInspector(ListRuleInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new ListRuleInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other))
            {
                ListElement = this[0] as ListElement;
                return true;
            }
            else
                return false;
        }

        public override CompileRuleInspector[] GetActiveSubInspectors()
        {
            CompileRuleInspector[] childInspectors = new CompileRuleInspector[ListElement.Count];
            for (int i = 0; i < ListElement.Count; i++)
            {
                childInspectors[i] = ListElement[i][0] as CompileRuleInspector;
            }
            return childInspectors;
        }

        /// <summary>
        /// Ensure a minimum number of list elements.
        /// </summary>
        public void EnsureElements(int count)
        {
            while (ListElement.Count < count)
            {
                ListElement.Add();
            }
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            base._Process(delta);

            for (int i = 0; i < ListElement.Count; i++)
            {
                ListElement[i].HeaderText = $"{Definition.Type.DisplayName} #{i + 1}";;
            }
        }

        /* Protected methods. */
        protected override void Init()
        {
            // Base compile rule inspector init.
            base.Init();

            // Set name.
            Name = $"ListRule ({Definition.ID})";

            // Add list element.
            ListElement = new()
            {
                HeaderText = Definition.DisplayName,
                AddButtonText = Definition.AddButtonText,
                Template = Create(Root, Definition.Type)
            };
            Add(ListElement);
        }
    }
}
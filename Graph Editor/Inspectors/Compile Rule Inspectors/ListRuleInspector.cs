using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
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
        public ListRule Rule => Resource as ListRule;

        /* Private properties. */
        private ListElement ListElement { get; set; }

        /* Constructors. */
        public ListRuleInspector() : base() { }

        public ListRuleInspector(InstructionSet instructionSet, ListRule compileRule)
            : base(instructionSet, compileRule) { }

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

        /* Protected methods. */
        protected override void Init(CompileRule resource)
        {
            // Base compile rule inspector init.
            base.Init(resource);

            // Set name.
            Name = $"ListRule ({resource.Id})";

            // Add list element.
            ListElement = new ListElement();
            ListElement.HeaderText = resource.DisplayName;
            ListElement.Template = Create(InstructionSet, Rule.Type);
            Add(ListElement);
        }
    }
}
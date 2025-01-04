using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A cutscene instruction inspector.
    /// </summary>
    public partial class NodeInstructionInspector : InstructionInspector
    {
        /* Public methods. */
        public string LabelName
        {
            get => Label.Value;
            set => Label.Value = value;
        }

        /* Private properties. */
        private LabeledIcon Title { get; set; }
        private LineField Label { get; set; }

        /* Constructors. */
        public NodeInstructionInspector() : base() { }

        public NodeInstructionInspector(InstructionSet instructionSet, InstructionDefinition resource)
            : base(instructionSet, resource) { }

        public NodeInstructionInspector(NodeInstructionInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new NodeInstructionInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is InstructionInspector otherInspector)
            {
                // Find title element and label field.
                Title = this[0] as LabeledIcon;
                Label = this[1] as LineField;

                return true;
            }
            return false;
        }

        /* Protected methods. */
        protected override void Init()
        {
            // Base inspector init.
            base.Init();

            // Set name.
            Name = $"NodeInstructionInspector ({Definition.Opcode})";

            // Add title text.
            Title = new();
            Title.LabelText = Definition.DisplayName;
            Title.Value = Definition.Icon;
            Title.Name = "Title";
            InsertAt(0, Title);

            // Add label field.
            Label = new();
            Label.LabelText = "Label";
            Label.Name = "Name";
            InsertAt(1, Label);
        }
    }
}
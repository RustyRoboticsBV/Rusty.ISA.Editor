using Rusty.ISA;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor
{
    /// <summary>
    /// A instruction inspector.
    /// </summary>
    public partial class NodeInstructionInspector : InstructionInspector
    {
        /* Public methods. */
        public LineField Label { get; private set; }
        public string LabelName
        {
            get => Label.Value;
            set => Label.Value = value;
        }

        /* Private properties. */
        private LabeledIcon Title { get; set; }
        private HSeparatorElement Bottom { get; set; }

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
            // Base instruction inspector init.
            base.Init();

            // Set name.
            Name = $"NodeInstructionInspector ({Definition.Opcode})";

            // Add title text.
            Title = new()
            {
                Name = "Title",
                LabelText = Definition.DisplayName,
                Value = Definition.Icon
            };
            InsertAt(0, Title);

            // Add label field.
            Label = new()
            {
                Name = "Name",
                LabelText = "Label"
            };
            InsertAt(1, Label);

            // Add bottom separator.
            Bottom = new()
            {
                Name = "Bottom"
            };
            Add(Bottom);
        }
    }
}
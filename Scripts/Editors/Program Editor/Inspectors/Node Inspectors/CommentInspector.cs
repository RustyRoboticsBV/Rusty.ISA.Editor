using Rusty.ISA.Editor.Programs.Compiler;
using Rusty.ISA;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Programs
{
    /// <summary>
    /// A ISA resource inspector.
    /// </summary>
    public partial class CommentInspector : Inspector
    {
        /* Public properties. */
        /// <summary>
        /// The current value of the comment.
        /// </summary>
        public string CommentText
        {
            get => Field.Value;
            set => Field.Value = value;
        }
        /// <summary>
        /// The comment instruction definition.
        /// </summary>
        public InstructionDefinition Definition
        {
            get => Resource as InstructionDefinition;
            set => Resource = value;
        }

        /* Private properties. */
        private MultilineField Field { get; set; }

        /* Constructors. */
        public CommentInspector() : base() { }

        public CommentInspector(InstructionSet instructionSet) : base(instructionSet, instructionSet[BuiltIn.CommentOpcode]) { }

        public CommentInspector(CommentInspector other) : base(other) { }

        /* Public methods. */
        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is CommentInspector otherInspector)
            {
                CommentText = otherInspector.CommentText;
                return true;
            }
            else
                return false;
        }

        /* Protected methods. */
        /// <summary>
        /// Inspector initialization method.
        /// </summary>
        protected override void Init()
        {
            base.Init();

            MultilineParameter parameter = Definition.Parameters[Definition.GetParameterIndex(BuiltIn.CommentText)] as MultilineParameter;
            Add(new LabeledIcon()
            {
                Name = "Header",
                LabelText = Definition.DisplayName,
                Value = Definition.Icon
            });
            Field = new MultilineField()
            {
                Value = parameter.DefaultValue,
                CustomMinimumSize = new(0f, 128f)
            };
            Add(Field);
        }
    }
}
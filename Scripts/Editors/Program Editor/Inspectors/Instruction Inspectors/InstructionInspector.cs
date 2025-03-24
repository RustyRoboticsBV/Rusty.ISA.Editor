using Godot;
using System.Collections.Generic;
using Rusty.EditorUI;

namespace Rusty.ISA.ProgramEditor
{
    /// <summary>
    /// A instruction inspector.
    /// </summary>
    [GlobalClass]
    public partial class InstructionInspector : Inspector
    {
        /* Public properties */
        /// <summary>
        /// The instruction definition visualized by this inspector.
        /// </summary>
        public InstructionDefinition Definition
        {
            get => Resource as InstructionDefinition;
            set => Resource = value;
        }

        public PreInstructionsInspector PreInstructions { get; set; }
        public PostInstructionsInspector PostInstructions { get; set; }

        public InstructionPreview Preview { get; private set; } = new();
        public bool UpdatedPreview { get; private set; }

        /* Private properties. */
        private List<ParameterInspector> Parameters { get; set; } = new();

        /* Constructors. */
        public InstructionInspector() : base() { }

        public InstructionInspector(InstructionSet instructionSet, InstructionDefinition resource)
            : base(instructionSet, resource) { }

        public InstructionInspector(InstructionInspector other) : base(other) { }

        /* Public methods. */
        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is InstructionInspector otherInspector)
            {
                // Find parameter & compile rule inspectors.
                Parameters.Clear();
                PreInstructions = null;
                PostInstructions = null;
                for (int i = 0; i < Count; i++)
                {
                    if (this[i] is ParameterInspector parameter)
                    {
                        parameter.Root = this;
                        Parameters.Add(parameter);
                    }
                    else if (this[i] is PreInstructionsInspector pre)
                    {
                        PreInstructions = pre;
                        pre.Root = this;
                    }
                    else if (this[i] is PostInstructionsInspector post)
                    {
                        PostInstructions = post;
                        post.Root = this;
                    }
                }
                return true;
            }
            return false;
        }

        public override Element Duplicate()
        {
            return new InstructionInspector(this);
        }

        /// <summary>
        /// Get a parameter inspector.
        /// </summary>
        public ParameterInspector GetParameterInspector(int index)
        {
            return Parameters[index];
        }

        /// <summary>
        /// Get a parameter inspector.
        /// </summary>
        public ParameterInspector GetParameterInspector(string id)
        {
            int index = Definition.GetParameterIndex(id);
            if (index >= 0)
                return Parameters[index];
            return null;
        }

        /// <summary>
        /// Get a compile rule inspector.
        /// </summary>
        public CompileRuleInspector GetCompileRuleInspector(string id)
        {
            for (int i = 0; i < Definition.PreInstructions.Length; i++)
            {
                if (Definition.PreInstructions[i].ID == id)
                    return GetPreInstructionInspector(i);
            }
            for (int i = 0; i < Definition.PostInstructions.Length; i++)
            {
                if (Definition.PostInstructions[i].ID == id)
                    return GetPostInstructionInspector(i);
            }
            return null;
        }

        /// <summary>
        /// Get a pre-instruction inspector by its index.
        /// </summary>
        public CompileRuleInspector GetPreInstructionInspector(int index)
        {
            return PreInstructions.Inspectors[index];
        }

        /// <summary>
        /// Get a post-instruction inspector by its index.
        /// </summary>
        public CompileRuleInspector GetPostInstructionInspector(int index)
        {
            return PostInstructions.Inspectors[index];
        }

        /// <summary>
        /// Set the values of this inspector's nested parameter inspectors.
        /// </summary>
        public void SetArguments(InstructionInstance instance)
        {
            for (int i = 0; i < Parameters.Count && i < instance.Arguments.Length; i++)
            {
                Parameters[i].ValueObj = instance.Arguments[i];
            }
        }

        /// <summary>
        /// Get all output parameter inspectors of this instruction inspector and its secondary inspectors.
        /// </summary>
        public List<ParameterInspector> GetOutputs()
        {
            List<ParameterInspector> Outputs = new();

            // Get parameter outputs.
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (Parameters[i] is OutputParameterInspector output)
                    Outputs.Add(output);
            }

            // Get pre-instruction outputs.
            for (int i = 0; i < PreInstructions.Inspectors.Count; i++)
            {
                CompileRuleInspector rule = GetPreInstructionInspector(i);

                List<ParameterInspector> preOutputs = rule.GetOutputs();
                foreach (ParameterInspector output in preOutputs)
                {
                    Outputs.Add(output);
                }
            }

            // Get post-instruction outputs.
            for (int i = 0; i < PostInstructions.Inspectors.Count; i++)
            {
                CompileRuleInspector rule = GetPostInstructionInspector(i);

                List<ParameterInspector> postOutputs = rule.GetOutputs();
                foreach (ParameterInspector output in postOutputs)
                {
                    Outputs.Add(output);
                }
            }
            return Outputs;
        }

        /// <summary>
        /// Force a preview update of this instruction inspector and all sub-inspectors. Does not set any UpdatedPreview variable
        /// to true.
        /// </summary>
        public void ForcePreviewUpdate()
        {
            // First, force-update all child inspector previews.
            for (int i = 0; i < Parameters.Count; i++)
            {
                Parameters[i].ForcePreviewUpdate();
            }
            for (int i = 0; i < PreInstructions.Inspectors.Count; i++)
            {
                PreInstructions.Inspectors[i].ForcePreviewUpdate();
            }
            for (int i = 0; i < PostInstructions.Inspectors.Count; i++)
            {
                PostInstructions.Inspectors[i].ForcePreviewUpdate();
            }

            // Update our preview.
            Preview = new(this);
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            base._Process(delta);

            // Check if one of the child inspector had its preview updated.
            UpdatedPreview = false;
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (Parameters[i].UpdatedPreview)
                {
                    UpdatedPreview = true;
                    break;
                }
            }
            for (int i = 0; i < PreInstructions.Inspectors.Count; i++)
            {
                if (UpdatedPreview)
                    break;
                if (PreInstructions.Inspectors[i].UpdatedPreview)
                {
                    UpdatedPreview = true;
                    break;
                }
            }
            for (int i = 0; i < PostInstructions.Inspectors.Count; i++)
            {
                if (UpdatedPreview)
                    break;
                if (PostInstructions.Inspectors[i].UpdatedPreview)
                {
                    UpdatedPreview = true;
                    break;
                }
            }

            // Update preview (if necessary).
            if (UpdatedPreview)
                ForcePreviewUpdate();
        }

        /* Protected methods. */
        protected override void Init()
        {
            // Base inspector init.
            base.Init();

            // Set name.
            Name = $"InstructionInspector ({Definition.Opcode})";

            // Add parameters.
            foreach (Parameter parameter in Definition.Parameters)
            {
                ParameterInspector parameterInspector = ParameterInspector.Create(this, parameter);
                Parameters.Add(parameterInspector);
                Add(parameterInspector);
            }

            // Add compile rules.
            PreInstructions = new(this, Definition);
            Add(PreInstructions);
            PreInstructions.Populate(Definition.PreInstructions);

            PostInstructions = new(this, Definition);
            Add(PostInstructions);
            PostInstructions.Populate(Definition.PostInstructions);
        }
    }
}
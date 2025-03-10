using System.Collections.Generic;
using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A cutscene instruction inspector.
    /// </summary>
    public abstract partial class InstructionInspector : Inspector
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
                for (int i = 0; i < Count; i++)
                {
                    if (this[i] is ParameterInspector parameterInspector)
                        Parameters.Add(parameterInspector);
                    else if (this[i] is PreInstructionsInspector preInspector)
                        PreInstructions = preInspector;
                    else if (this[i] is PostInstructionsInspector postInspector)
                        PostInstructions = postInspector;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get a parameter inspector.
        /// </summary>
        public ParameterInspector GetParameterInspector(int index)
        {
            return Parameters[index];
        }

        /// <summary>
        /// Get a pre-instruction inspector by its index.
        /// </summary>
        public Inspector GetPreInstructionInspector(int index)
        {
            return PreInstructions.Inspectors[index];
        }

        /// <summary>
        /// Get a post-instruction inspector by its index.
        /// </summary>
        public Inspector GetPostInstructionInspector(int index)
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
                Inspector inspector = GetPreInstructionInspector(i);

                List<ParameterInspector> preOutputs = new();
                if (inspector is InstructionRuleInspector pre)
                    preOutputs = pre.GetOutputs();
                else if (inspector is CompileRuleInspector rule)
                    preOutputs = rule.GetOutputs();

                foreach (ParameterInspector output in preOutputs)
                {
                    Outputs.Add(output);
                }
            }

            // Get post-instruction outputs.
            for (int i = 0; i < PostInstructions.Inspectors.Count; i++)
            {
                Inspector inspector = GetPostInstructionInspector(i);

                List<ParameterInspector> postOutputs = new();
                if (inspector is InstructionRuleInspector post)
                    postOutputs = post.GetOutputs();
                else if (inspector is CompileRuleInspector rule)
                    postOutputs = rule.GetOutputs();

                foreach (ParameterInspector output in postOutputs)
                {
                    Outputs.Add(output);
                }
            }
            return Outputs;
        }

        public string[] GetPreviewTerms()
        {
            return new string[0];
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
                ParameterInspector parameterInspector = ParameterInspector.Create(InstructionSet, parameter);
                Parameters.Add(parameterInspector);
                Add(parameterInspector);
            }

            // Add compile rules.
            PreInstructions = new(InstructionSet, Definition);
            Add(PreInstructions);
            PreInstructions.Populate(Definition.PreInstructions);

            PostInstructions = new(InstructionSet, Definition);
            Add(PostInstructions);
            PostInstructions.Populate(Definition.PostInstructions);
        }
    }
}
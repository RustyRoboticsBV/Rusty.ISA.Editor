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

        /* Private properties. */
        private List<ParameterInspector> Parameters { get; set; } = new();
        private List<Inspector> CompileRules { get; set; } = new();

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
                for (int i = 0; i < Count; i++)
                {
                    if (this[i] is ParameterInspector parameterInspector)
                        Parameters.Add(parameterInspector);
                    else if (this[i] is CompileRuleInspector compileRuleInspector)
                        CompileRules.Add(compileRuleInspector);
                }

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
            Name = $"InstructionInspector ({Definition.Opcode})";

            // Add parameters.
            foreach (ParameterDefinition parameter in Definition.Parameters)
            {
                ParameterInspector parameterInspector = ParameterInspector.Create(InstructionSet, parameter);
                Parameters.Add(parameterInspector);
                Add(parameterInspector);
            }

            // Add compile rules.
            foreach (CompileRule compileRule in Definition.PreInstructions)
            {
                Inspector compileRuleInspector = CompileRuleInspector.Create(InstructionSet, compileRule);
                CompileRules.Add(compileRuleInspector);
                Add(compileRuleInspector);
            }
        }
    }
}
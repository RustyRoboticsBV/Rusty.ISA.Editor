using System;
using System.Collections.Generic;
using Godot;
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

        /// <summary>
        /// The number of compile rule inspectors nested within this inspector.
        /// </summary>
        public int CompileRuleInspectorCount => CompileRules.Count;

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
                Parameters.Clear();
                CompileRules.Clear();
                for (int i = 0; i < Count; i++)
                {
                    if (this[i] is ParameterInspector parameterInspector)
                        Parameters.Add(parameterInspector);
                    else if (this[i] is CompileRuleInspector compileRuleInspector)
                        CompileRules.Add(compileRuleInspector);
                    else if (this[i] is PreInstructionInspector preInstructionInspector)
                        CompileRules.Add(preInstructionInspector);
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
        /// Get a compile rule inspector.
        /// </summary>
        public Inspector GetCompileRuleInspector(int index)
        {
            return CompileRules[index];
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
        /// Get all output parameter inspectors of this instruction inspector and its associated compile rule inspectors.
        /// </summary>
        public List<ParameterInspector> GetOutputs()
        {
            List<ParameterInspector> Outputs = new();
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (Parameters[i] is OutputParameterInspector output)
                    Outputs.Add(output);
            }

            for (int i = 0; i < CompileRules.Count; i++)
            {
                List<ParameterInspector> preOutputs = new();
                if (CompileRules[i] is PreInstructionInspector pre)
                    preOutputs = pre.GetOutputs();
                else if (CompileRules[i] is CompileRuleInspector rule)
                    preOutputs = rule.GetOutputs();

                foreach (ParameterInspector output in preOutputs)
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
            foreach (ParameterDefinition parameter in Definition.Parameters)
            {
                ParameterInspector parameterInspector = ParameterInspector.Create(InstructionSet, parameter);
                Parameters.Add(parameterInspector);
                Add(parameterInspector);
            }

            // Add compile rules.
            foreach (CompileRule compileRule in Definition.CompileRules)
            {
                Inspector compileRuleInspector = CompileRuleInspector.Create(InstructionSet, compileRule);
                CompileRules.Add(compileRuleInspector);
                Add(compileRuleInspector);
            }
        }
    }
}
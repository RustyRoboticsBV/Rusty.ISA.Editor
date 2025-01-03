using System.Collections.Generic;
using Godot;
using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A cutscene instruction inspector.
    /// </summary>
    public partial class InstructionInspector : Inspector<InstructionDefinition>
    {
        /* Private properties. */
        private LabeledIcon Title { get; set; }
        private LineField Label { get; set; }
        private List<ParameterInspector> Parameters { get; set; } = new();
        private List<CompileRuleInspector> CompileRules { get; set; } = new();
        private bool PreInstruction { get; set; }

        /* Constructors. */
        public InstructionInspector() : base() { }

        public InstructionInspector(InstructionSet instructionSet, InstructionDefinition resource)
            : base(instructionSet, resource) { }

        public InstructionInspector(InstructionInspector other) : base(other) { }

        /* Public methods. */
        public void RemoveTitleAndLabel()
        {
            Remove(Title);
            Remove(Label);
            PreInstruction = true;
        }

        public override Element Duplicate()
        {
            return new InstructionInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is InstructionInspector otherInspector)
            {
                PreInstruction = otherInspector.PreInstruction;

                // Find title element and label field.
                if (!PreInstruction)
                {
                    Title = this[0] as LabeledIcon;
                    Label = this[1] as LineField;
                }

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
        protected override void Init(InstructionDefinition resource)
        {
            // Base inspector init.
            base.Init(resource);

            // Set name.
            Name = $"InstructionInspector ({resource.Opcode})";

            // Add title text.
            Title = new();
            Title.LabelText = resource.DisplayName;
            Title.Value = resource.Icon;
            Title.Name = "Title";
            Add(Title);

            // Add label field.
            Label = new();
            Label.LabelText = "Label";
            Label.Name = "Name";
            Add(Label);

            // Add parameters.
            foreach (ParameterDefinition parameter in resource.Parameters)
            {
                ParameterInspector parameterInspector = ParameterInspector.Create(InstructionSet, parameter);
                Parameters.Add(parameterInspector);
                Add(parameterInspector);
            }

            // Add compile rules.
            foreach (CompileRule compileRule in resource.PreInstructions)
            {
                CompileRuleInspector compileRuleInspector = CompileRuleInspector.Create(InstructionSet, compileRule);
                CompileRules.Add(compileRuleInspector);
                Add(compileRuleInspector);
            }
        }
    }
}
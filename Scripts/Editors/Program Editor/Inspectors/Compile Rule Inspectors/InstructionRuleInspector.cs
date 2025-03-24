using Godot;
using Rusty.EditorUI;
using System.Collections.Generic;

namespace Rusty.ISA.ProgramEditor
{
    /// <summary>
    /// A pre-instruction inspector.
    /// </summary>
    public partial class InstructionRuleInspector : CompileRuleInspector
    {
        /* Public properties. */
        /// <summary>
        /// The compile rule visualized by this inspector.
        /// </summary>
        public new InstructionRule Definition => base.Definition as InstructionRule;

        /// <summary>
        /// The instruction's inspector.
        /// </summary>
        public InstructionInspector TargetInstruction { get; private set; }

        /* Constructors. */
        public InstructionRuleInspector(InstructionInspector root, InstructionRule resource)
            : base(root, resource) { }

        public InstructionRuleInspector(InstructionRuleInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new InstructionRuleInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other))
            {
                TargetInstruction = this[0] as InstructionInspector;
                return true;
            }
            else
                return false;
        }

        public override CompileRuleInspector[] GetActiveSubInspectors()
        {
            return new CompileRuleInspector[0];
        }

        public override List<ParameterInspector> GetOutputs()
        {
            return TargetInstruction.GetOutputs();
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            base._Process(delta);

            if (!UpdatedPreview && TargetInstruction != null && TargetInstruction.UpdatedPreview)
            {
                ForcePreviewUpdate();
                UpdatedPreview = true;
            }
        }

        /* Protected methods. */
        protected override void Init()
        {
            base.Init();

            TargetInstruction = new InstructionInspector(InstructionSet, InstructionSet[Definition.Opcode]);
            Add(TargetInstruction);
            TargetInstruction.Name = "Instruction";
        }
    }
}
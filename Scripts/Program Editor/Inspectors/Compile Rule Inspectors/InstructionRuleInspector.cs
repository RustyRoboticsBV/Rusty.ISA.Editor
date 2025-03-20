using Godot;
using Rusty.EditorUI;
using System.Collections.Generic;

namespace Rusty.ISA.Editor
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
        public InstructionRule Rule => Resource as InstructionRule;

        /// <summary>
        /// The instruction's inspector.
        /// </summary>
        public InstructionInspector InstructionInspector { get; private set; }

        /* Constructors. */
        public InstructionRuleInspector() : base() { }

        public InstructionRuleInspector(InstructionSet instructionSet, InstructionRule resource)
            : base(instructionSet, resource) { }

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
                InstructionInspector = this[0] as InstructionInspector;
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
            return InstructionInspector.GetOutputs();
        }

        /* Protected methods. */
        protected override void Init()
        {
            base.Init();

            InstructionInspector = new InstructionInspector(InstructionSet, InstructionSet[Rule.Opcode]);
            Add(InstructionInspector);
            InstructionInspector.Name = "Instruction";
        }
    }
}
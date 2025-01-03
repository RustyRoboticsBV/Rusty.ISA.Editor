using System;
using Rusty.Cutscenes;
using Rusty.Cutscenes.Editor;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A cutscene instruction compile rule inspector.
    /// </summary>
    public abstract partial class CompileRuleInspector : Inspector
    {
        /* Constructors. */
        public CompileRuleInspector() : base() { }

        public CompileRuleInspector(InstructionSet instructionSet, CompileRule compileRule)
            : base(instructionSet, compileRule) { }

        public CompileRuleInspector(CompileRuleInspector other) : base(other) { }

        /* Public methods. */
        /// <summary>
        /// Gets the child rule inspector(s) that are currently active, if any.
        /// </summary>
        public abstract Inspector[] GetActiveSubInspectors();

        /// <summary>
        /// Create a compile rule inspector of some type.
        /// </summary>
        public static Inspector Create(InstructionSet instructionSet, CompileRule compileRule)
        {
            switch (compileRule)
            {
                case OptionRule optionRule:
                    return new OptionRuleInspector(instructionSet, optionRule);
                case ChoiceRule choiceRule:
                    return new ChoiceRuleInspector(instructionSet, choiceRule);
                case TupleRule tupleRule:
                    return new TupleRuleInspector(instructionSet, tupleRule);
                case ListRule listRule:
                    return new ListRuleInspector(instructionSet, listRule);
                case PreInstruction preInstruction:
                    return new PreInstructionInspector(instructionSet, instructionSet[preInstruction.Opcode]);
                default:
                    throw new ArgumentException($"Compile rule '{compileRule}' has an illegal type '{compileRule.GetType().Name}'.");
            }
        }
    }
}
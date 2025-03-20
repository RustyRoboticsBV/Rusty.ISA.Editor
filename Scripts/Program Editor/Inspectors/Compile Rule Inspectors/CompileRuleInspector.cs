using System;
using System.Collections.Generic;

namespace Rusty.ISA.Editor
{
    /// <summary>
    /// A instruction compile rule inspector.
    /// </summary>
    public abstract partial class CompileRuleInspector : Inspector
    {
        /* Public properties. */
        public CompileRule Definition => Resource as CompileRule;

        /* Constructors. */
        public CompileRuleInspector() : base() { }

        public CompileRuleInspector(InstructionSet instructionSet, CompileRule compileRule)
            : base(instructionSet, compileRule) { }

        public CompileRuleInspector(CompileRuleInspector other) : base(other) { }

        /* Public methods. */
        /// <summary>
        /// Gets the child rule inspector(s) that are currently active, if any.
        /// </summary>
        public abstract CompileRuleInspector[] GetActiveSubInspectors();

        /// <summary>
        /// Create a compile rule inspector of some type.
        /// </summary>
        public static CompileRuleInspector Create(InstructionSet instructionSet, CompileRule compileRule)
        {
            switch (compileRule)
            {
                case InstructionRule instructionRule:
                    return new InstructionRuleInspector(instructionSet, instructionRule);
                case OptionRule optionRule:
                    return new OptionRuleInspector(instructionSet, optionRule);
                case ChoiceRule choiceRule:
                    return new ChoiceRuleInspector(instructionSet, choiceRule);
                case TupleRule tupleRule:
                    return new TupleRuleInspector(instructionSet, tupleRule);
                case ListRule listRule:
                    return new ListRuleInspector(instructionSet, listRule);
                default:
                    throw new ArgumentException($"Compile rule '{compileRule}' has an illegal type '{compileRule.GetType().Name}'.");
            }
        }

        /// <summary>
        /// Get all output parameter inspectors associated with this compile rule inspector and its sub-inspectors.
        /// </summary>
        public virtual List<ParameterInspector> GetOutputs()
        {
            List<ParameterInspector> Outputs = new();
            CompileRuleInspector[] subInspectors = GetActiveSubInspectors();
            foreach (CompileRuleInspector rule in subInspectors)
            {
                List<ParameterInspector> subOutputs = rule.GetOutputs();
                foreach (ParameterInspector output in subOutputs)
                {
                    Outputs.Add(output);
                }
            }
            return Outputs;
        }
    }
}
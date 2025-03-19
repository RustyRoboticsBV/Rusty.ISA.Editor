﻿using System;
using System.Collections.Generic;
using Rusty.ISA;

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
        public abstract Inspector[] GetActiveSubInspectors();

        /// <summary>
        /// Create a compile rule inspector of some type.
        /// </summary>
        public static Inspector Create(InstructionSet instructionSet, CompileRule compileRule)
        {
            switch (compileRule)
            {
                case InstructionRule preInstruction:
                    return new InstructionRuleInspector(instructionSet, instructionSet[preInstruction.Opcode]);
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
        public List<ParameterInspector> GetOutputs()
        {
            List<ParameterInspector> Outputs = new();
            Inspector[] subInspectors = GetActiveSubInspectors();
            for (int i = 0; i < subInspectors.Length; i++)
            {
                List<ParameterInspector> preOutputs = new();
                if (subInspectors[i] is InstructionRuleInspector pre)
                    preOutputs = pre.GetOutputs();
                else if (subInspectors[i] is CompileRuleInspector rule)
                    preOutputs = rule.GetOutputs();

                foreach (ParameterInspector output in preOutputs)
                {
                    Outputs.Add(output);
                }
            }
            return Outputs;
        }
    }
}
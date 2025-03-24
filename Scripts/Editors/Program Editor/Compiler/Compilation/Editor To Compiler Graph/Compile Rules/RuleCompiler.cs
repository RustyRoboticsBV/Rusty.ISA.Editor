using System;
using Rusty.Graphs;

namespace Rusty.ISA.Editor.Compiler
{
    /// <summary>
    /// A base class for rule inspector compilers.
    /// </summary>
    public abstract class RuleCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile a rule inspector into a compiler node hierarchy.
        /// </summary>
        public static SubNode<NodeData> Compile(Inspector inspector)
        {
            switch (inspector)
            {
                case InstructionRuleInspector preInspector:
                    return InspectorCompiler.Compile(preInspector.TargetInstruction);
                case OptionRuleInspector optionInspector:
                    return OptionRuleCompiler.Compile(optionInspector);
                case ChoiceRuleInspector choiceInspector:
                    return ChoiceRuleCompiler.Compile(choiceInspector);
                case TupleRuleInspector tupleInspector:
                    return TupleRuleCompiler.Compile(tupleInspector);
                case ListRuleInspector listInspector:
                    return ListRuleCompiler.Compile(listInspector);
                default:
                    throw new ArgumentException($"The inspector '{inspector.Name}' was of illegal type '{inspector.GetType().Name}'.");
            }
        }
    }
}
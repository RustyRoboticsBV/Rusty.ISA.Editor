using System;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// A base class for rule inspector compilers.
    /// </summary>
    public abstract class RuleCompiler : Compiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile a rule inspector into a compiler node hierarchy.
        /// </summary>
        public static SubNode<NodeData>[] Compile(Inspector inspector)
        {
            switch (inspector)
            {
                case InstructionRuleInspector preInspector:
                    return InstructionCompiler.Compile(preInspector);
                case OptionRuleInspector optionInspector:
                    return new SubNode<NodeData>[] { OptionRuleCompiler.Compile(optionInspector) };
                case ChoiceRuleInspector choiceInspector:
                    return new SubNode<NodeData>[] { ChoiceRuleCompiler.Compile(choiceInspector) };
                case TupleRuleInspector tupleInspector:
                    return new SubNode<NodeData>[] { TupleRuleCompiler.Compile(tupleInspector) };
                case ListRuleInspector listInspector:
                    return new SubNode<NodeData>[] { ListRuleCompiler.Compile(listInspector) };
                default:
                    throw new ArgumentException($"The inspector '{inspector.Name}' was of illegal type '{inspector.GetType().Name}'.");
            }
        }
    }
}
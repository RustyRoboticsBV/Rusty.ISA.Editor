using System;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    public abstract class RuleCompiler : Compiler
    {
        /* Public methods. */
        public static SubNode<NodeData> Compile(Inspector inspector)
        {
            switch (inspector)
            {
                case PreInstructionInspector preInspector:
                    return InstructionCompiler.Compile(preInspector);
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
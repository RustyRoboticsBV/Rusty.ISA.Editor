namespace Rusty.ISA.Editor;

public static class RuleInspectorFactory
{
    public static RuleInspector Create(InstructionSet set, CompileRule rule)
    {
        // Create inspector.
        RuleInspector inspector = new();

        switch (rule)
        {
            case InstructionRule i:
                return new InstructionRuleInspector(set, i);

            case OptionRule o:
                return new OptionRuleInspector(set, o);

            case ChoiceRule c:
                return new ChoiceRuleInspector(set, c);

            case TupleRule t:
                return new TupleRuleInspector(set, t);

            case ListRule l:
                return new ListRuleInspector(set, l);
        }

        inspector.TooltipText = rule.Description;

        return inspector;
    }
}
namespace Rusty.ISA.Editor;

public static class RuleInspectorFactory
{
    public static RuleInspector Create(InstructionSet set, CompileRule rule)
    {
        // Create inspector.
        RuleInspector inspector = null;

        switch (rule)
        {
            case InstructionRule i:
                inspector = new InstructionRuleInspector(set, i);
                break;

            case OptionRule o:
                inspector = new OptionRuleInspector(set, o);
                break;

            case ChoiceRule c:
                inspector = new ChoiceRuleInspector(set, c);
                break;

            case TupleRule t:
                inspector = new TupleRuleInspector(set, t);
                break;

            case ListRule l:
                inspector = new ListRuleInspector(set, l);
                break;
        }

        if (inspector != null)
            inspector.TooltipText = rule.Description;

        return inspector;
    }
}
namespace Rusty.ISA.Editor
{
    public class CompileRulePreview : Preview<CompileRuleInspector>
    {
        /* Public properties. */
        public InstructionPreview Parent { get; private set; }

        /* Constructors. */
        public CompileRulePreview(InstructionPreview parent, CompileRuleInspector inspector)
            : base(parent.Inspector, inspector, inspector.Definition.Preview)
        {
            Parent = parent;
        }

        /* Public methids. */
        public static string Parse(InstructionInspector parent, string ruleID)
        {
            return "";
        }

        /* Protected methods. */
        protected override string GetDebugName()
        {
            return Inspector.Definition.GetType().Name + " " + Inspector.Definition.ID;
        }

        protected override string GetElements()
        {
            string str = "";
            switch (Inspector)
            {
                case TupleRuleInspector:
                case ListRuleInspector:
                    CompileRuleInspector[] items = Inspector.GetActiveSubInspectors();
                    foreach (var item in items)
                    {
                        if (str != "")
                            str += ", ";
                        CompileRulePreview preview = new(Parent, item);
                        str += Make(preview.Evaluate());
                    }
                    break;

                default:
                    str = base.GetElements();
                    break;
            }
            return str;
        }

        protected override string GetDefaultExpression()
        {
            switch (Inspector)
            {
                case InstructionRuleInspector instruction:
                    return Parse(instruction);

                case OptionRuleInspector option:
                    return Parse(Parent, option);

                case ChoiceRuleInspector choice:
                    return Parse(Parent, choice);

                case TupleRuleInspector tuple:
                    CompileRuleInspector[] items = tuple.GetActiveSubInspectors();
                    string concat = "";
                    foreach (var item in items)
                    {
                        if (concat != "")
                            concat += " + ";
                        CompileRulePreview preview = new(Parent, item);
                        concat += Make(" " + preview.Evaluate());
                    }
                    return concat;

                case ListRuleInspector list:
                    CompileRuleInspector[] elements = list.GetActiveSubInspectors();
                    string sum = "";
                    foreach (var element in elements)
                    {
                        if (sum != "")
                            sum += " + ";
                        CompileRulePreview preview = new(Parent, element);
                        sum += Make("\n" + preview.Evaluate());
                    }
                    return sum;
            }

            return "";
        }

        protected override string ParseParameter(string parameterID)
        {
            for (int i = 0; i < Parent.Parameters.Count; i++)
            {
                if (Parent.Parameters[i].Inspector.Definition.ID == parameterID)
                    return Make(Parent.Parameters[i].Evaluate());
            }
            return GetParameterError(parameterID);
        }

        protected override string ParseCompileRule(string ruleID)
        {
            if (Inspector is InstructionRuleInspector instruction && ruleID == "target")
                return Parse(instruction);
            else if (Inspector is OptionRuleInspector option && ruleID == "option")
                return Parse(Parent, option);
            else if (Inspector is ChoiceRuleInspector choice && ruleID == "selected")
                return Parse(Parent, choice);
            else if (Inspector is TupleRuleInspector tuple)
            {
                for (int i = 0; i < tuple.Rule.Types.Length; i++)
                {
                    if (tuple.Rule.Types[i].ID == ruleID)
                        return Make(new CompileRulePreview(Parent, tuple.GetActiveSubInspectors()[i]).Evaluate());
                }
            }
            else if (Inspector is ListRuleInspector list)
            {
                if (ruleID == "count")
                    return list.GetActiveSubInspectors().Length.ToString();
                else if (ruleID.StartsWith("element"))
                {
                    string index = ruleID.Substring("element".Length);
                    return $"_elements[{index}]";
                }
            }

            return InstructionPreview.ParseCompileRule(Parent, ruleID);
        }

        /* Private methods. */
        private static string Parse(InstructionRuleInspector inspector)
        {
            return Make(new InstructionPreview(inspector.InstructionInspector).Evaluate());
        }

        private static string Parse(InstructionPreview parent, OptionRuleInspector option)
        {
            if (option.Checked)
                return Make(new CompileRulePreview(parent, option.ChildRuleInspector).Evaluate());
            else
                return "";
        }

        private static string Parse(InstructionPreview parent, ChoiceRuleInspector choice)
        {
            return Make(new CompileRulePreview(parent, choice.GetSelected()).Evaluate());
        }
    }
}
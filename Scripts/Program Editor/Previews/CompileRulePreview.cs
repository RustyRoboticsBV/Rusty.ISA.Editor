namespace Rusty.ISA.Editor
{
    public class CompileRulePreview : Preview<CompileRuleInspector>
    {
        /* Public properties. */
        /// <summary>
        /// The instruction inspector that acts as the root of this preview's inspector.
        /// </summary>
        public InstructionInspector Root => Inspector.Root;

        /* Constructors. */
        public CompileRulePreview() : base() { }

        public CompileRulePreview(CompileRuleInspector inspector)
            : base(inspector, inspector.Definition.Preview) { }

        /* Protected methods. */
        protected override string GetDebugName()
        {
            if (Inspector != null)
                return Inspector.Definition.GetType().Name + " " + Inspector.Definition.ID;
            else
                return "CompileRule null";
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
                        CompileRulePreview preview = new(item);
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
            if (Inspector == null || Root == null)
                return "";

            switch (Inspector)
            {
                case InstructionRuleInspector instruction:
                    return Parse(instruction);

                case OptionRuleInspector option:
                    return Parse(option);

                case ChoiceRuleInspector choice:
                    return Parse(choice);

                case TupleRuleInspector tuple:
                    CompileRuleInspector[] items = tuple.GetActiveSubInspectors();
                    string concat = "";
                    foreach (var item in items)
                    {
                        if (concat != "")
                            concat += " + ";
                        CompileRulePreview preview = new(item);
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
                        CompileRulePreview preview = new(element);
                        sum += Make("\n" + preview.Evaluate());
                    }
                    return sum;
            }

            return "";
        }

        protected override string ParseParameter(string parameterID)
        {
            ParameterInspector inspector = Root.GetParameterInspector(parameterID);
            if (inspector != null)
                return Make(inspector.Preview.Evaluate());
            else
                return GetParameterError(parameterID);
        }

        protected override string ParseCompileRule(string ruleID)
        {
            // Special keywords.
            if (Inspector is InstructionRuleInspector instruction && ruleID == "target")
                return Parse(instruction);
            else if (Inspector is OptionRuleInspector option && ruleID == "option")
                return Parse(option);
            else if (Inspector is ChoiceRuleInspector choice && ruleID == "selected")
                return Parse(choice);
            else if (Inspector is TupleRuleInspector tuple)
            {
                for (int i = 0; i < tuple.Definition.Types.Length; i++)
                {
                    if (tuple.Definition.Types[i].ID == ruleID)
                        return Make(tuple.GetActiveSubInspectors()[i].Preview.Evaluate());
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

            // Else, we want to get the preview of another compile rule.
            CompileRuleInspector inspector = Root.GetCompileRuleInspector(ruleID);
            if (inspector != null)
                return Make(inspector.Preview.Evaluate());
            else
                return GetRuleError(ruleID);
        }

        /* Private methods. */
        private static string Parse(InstructionRuleInspector inspector)
        {
            return Make(inspector.TargetInstruction.Preview.Evaluate());
        }

        private static string Parse(OptionRuleInspector option)
        {
            if (option.Checked)
                return Make(option.ChildRuleInspector.Preview.Evaluate());
            else
                return "";
        }

        private static string Parse(ChoiceRuleInspector choice)
        {
            return Make(choice.Preview.Evaluate());
        }
    }
}
using Godot;

namespace Rusty.ISA.Editor
{
    public class ParameterPreview : Preview<ParameterInspector>
    {
        /* Public properties. */
        public InstructionInspector Parent { get; private set; }

        /* Constructors. */
        public ParameterPreview(InstructionInspector parent, ParameterInspector inspector) : base(inspector)
        {
            Parent = parent;
        }

        /* Public methids. */
        public static string Parse(InstructionInspector parent, string parameterID)
        {
            InstructionDefinition definition = parent.Definition;
            int index = definition.GetParameterIndex(parameterID);

            string result = "";
            if (index >= 0)
            {
                ParameterPreview preview = new(parent, parent.GetParameterInspector(index));
                result = preview.Generate(definition.Parameters[index].Preview);
            }
            else
                result = $"bad_parameter({parameterID})";

            return MakeExpression(result);
        }

        /* Protected methods. */
        protected override string GetDefault()
        {
            return MakeExpression(Inspector.ValueObj);
        }

        protected override void ParseParameter(ref string expression, ref int startIndex, int length, string parameterID)
        {
            int index = Parent.Definition.GetParameterIndex(parameterID);

            // Get replacement string.
            string replace = "";
            if (index >= 0)
                replace = Parent.GetParameterInspector(index).ValueObj.ToString();
            else
                replace = $"bad_parameter({parameterID})";

            // Enclose in quotes.
            replace = MakeExpression(replace);

            // Alter expression.
            Replace(ref expression, ref startIndex, length, replace);
        }

        protected override void ParseCompileRule(ref string expression, ref int startIndex, int length, string ruleID)
        {
            Replace(ref expression, ref startIndex, length, CompileRulePreview.Parse(Parent, $"[{ruleID}]"));
        }
    }
}
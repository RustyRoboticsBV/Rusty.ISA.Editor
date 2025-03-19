using Godot;

namespace Rusty.ISA.Editor
{
    public class InstructionPreview : Preview<InstructionInspector>
    {
        /* Constructors. */
        public InstructionPreview(InstructionInspector inspector) : base(inspector) { }

        /* Public methods. */
        public static string Parse(InstructionInspector inspector, string expression)
        {
            InstructionPreview preview = new(inspector);
            return preview.Generate(expression);
        }

        /* Protected methods. */
        protected override string GetDefault()
        {
            return "";
        }

        protected override void ParseParameter(ref string expression, ref int startIndex, int length, string parameterID)
        {
            GD.Print("Encountered parameter " + parameterID);
            Replace(ref expression, ref startIndex, length, ParameterPreview.Parse(Inspector, parameterID));
        }

        protected override void ParseCompileRule(ref string expression, ref int startIndex, int length, string ruleID)
        {
            GD.Print("TODO: parse compile rule " + ruleID);
            Replace(ref expression, ref startIndex, length, CompileRulePreview.Parse(Inspector, ruleID));
        }
    }
}
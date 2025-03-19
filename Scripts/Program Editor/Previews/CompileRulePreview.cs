using Godot;

namespace Rusty.ISA.Editor
{
    public class CompileRulePreview : Preview<CompileRuleInspector>
    {
        /* Public properties. */
        public InstructionInspector Parent { get; private set; }

        /* Constructors. */
        public CompileRulePreview(InstructionInspector parent, CompileRuleInspector inspector) : base(inspector)
        {
            Parent = parent;
        }

        /* Public methids. */
        public static string Parse(InstructionInspector parent, string ruleID)
        {
            return "";
        }

        /* Protected methods. */
        protected override string GetDefault()
        {
            return "";
        }

        protected override void ParseParameter(ref string expression, ref int startIndex, int length, string parameterID)
        {
            Replace(ref expression, ref startIndex, length, ParameterPreview.Parse(Parent, parameterID));
        }

        protected override void ParseCompileRule(ref string expression, ref int startIndex, int length, string id)
        {
            Replace(ref expression, ref startIndex, length, "");
        }
    }
}
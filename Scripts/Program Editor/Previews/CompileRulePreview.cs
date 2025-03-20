using Godot;

namespace Rusty.ISA.Editor
{
    public class CompileRulePreview : Preview<CompileRuleInspector>
    {
        /* Public properties. */
        public InstructionInspector Parent { get; private set; }

        /* Constructors. */
        public CompileRulePreview(InstructionInspector parent, CompileRuleInspector inspector)
            : base(parent, inspector, inspector.Definition.Preview)
        {
            Parent = parent;
        }

        /* Public methids. */
        public static string Parse(InstructionInspector parent, string ruleID)
        {
            return "";
        }

        /* Protected methods. */
        protected override string GetDefaultExpression()
        {
            return "";
        }

        protected override string ParseParameter(string parameterID)
        {
            return "";
        }

        protected override string ParseCompileRule(string ruleID)
        {
            return "";
        }
    }
}
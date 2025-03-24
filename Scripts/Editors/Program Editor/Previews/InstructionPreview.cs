namespace Rusty.ISA.ProgramEditor
{
    public class InstructionPreview : Preview<InstructionInspector>
    {
        /* Constructors. */
        public InstructionPreview() : base() { }

        public InstructionPreview(InstructionInspector inspector)
            : base(inspector, inspector.Definition.Preview) { }

        /* Protected methods. */
        protected override string GetDebugName()
        {
            if (Inspector != null)
                return "InstructionDefinition " + Inspector.Definition.Opcode;
            else
                return "InstructionDefinition null";
        }

        protected override string GetDefaultExpression()
        {
            return "";
        }

        protected override string ParseParameter(string parameterID)
        {
            ParameterInspector inspector = Inspector.GetParameterInspector(parameterID);
            if (inspector != null)
                return Make(inspector.Preview.Evaluate());
            else
                return GetParameterError(parameterID);
        }

        protected override string ParseCompileRule(string ruleID)
        {
            CompileRuleInspector inspector = Inspector.GetCompileRuleInspector(ruleID);
            if (inspector != null)
                return Make(inspector.Preview.Evaluate());
            else
                return GetRuleError(ruleID);
        }
    }
}
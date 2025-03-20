namespace Rusty.ISA.Editor
{
    public class ParameterPreview : Preview<ParameterInspector>
    {
        /* Public properties. */
        public InstructionPreview Parent { get; private set; }

        /* Constructors. */
        public ParameterPreview(InstructionPreview parent, ParameterInspector inspector)
            : base(parent.Inspector, inspector, inspector.Definition.Preview)
        {
            Parent = parent;
        }

        /* Protected methods. */
        protected override string GetDebugName()
        {
            return Inspector.Definition.GetType().Name + " " + Inspector.Definition.ID;
        }

        protected override string GetDefaultExpression()
        {
            return Make(Inspector.ValueObj.ToString());
        }

        protected override string ParseParameter(string parameterID)
        {
            int index = Root.Definition.GetParameterIndex(parameterID);

            if (index >= 0)
                return Root.GetParameterInspector(index).GetValue();
            else
                return GetParameterError(parameterID);
        }

        protected override string ParseCompileRule(string ruleID)
        {
            return InstructionPreview.ParseCompileRule(Parent, ruleID);
        }
    }
}
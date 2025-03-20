using Godot;

namespace Rusty.ISA.Editor
{
    public class ParameterPreview : Preview<ParameterInspector>
    {
        /* Public properties. */
        /// <summary>
        /// The instruction inspector that acts as the root of this preview's inspector.
        /// </summary>
        public InstructionInspector Root => Inspector.Root;

        /* Constructors. */
        public ParameterPreview() : base() { }

        public ParameterPreview(ParameterInspector inspector)
            : base(inspector, inspector.Definition.Preview) { }

        /* Protected methods. */
        protected override string GetDebugName()
        {
            if (Inspector != null)
                return Inspector.Definition.GetType().Name + " " + Inspector.Definition.ID;
            else
                return "Parameter null";
        }

        protected override string GetDefaultExpression()
        {
            if (Inspector == null)
                return "";
            else
                return Make(Inspector.ValueObj.ToString());
        }

        protected override string ParseParameter(string parameterID)
        {
            GD.Print("Parsing parameter reference " + parameterID + " in parameter " + Inspector.Definition.ID);
            // If the id is our id, return our value.
            if (Inspector == null)
                return GetNullError("parameter_inspector");

            if (parameterID == Inspector.Definition.ID)
                return Make(Inspector.GetValue());

            // Else, look for the parameter in the root.
            if (Root == null)
                return GetNullError("parameter_root");

            int index = Root.Definition.GetParameterIndex(parameterID);

            if (index >= 0)
                return Make(Root.GetParameterInspector(index).GetValue());
            else
                return GetParameterError(parameterID);
        }

        protected override string ParseCompileRule(string ruleID)
        {
            CompileRuleInspector inspector = Root.GetCompileRuleInspector(ruleID);
            if (inspector != null)
                return Make(inspector.Preview.Evaluate());
            else
                return GetRuleError(ruleID);
        }
    }
}
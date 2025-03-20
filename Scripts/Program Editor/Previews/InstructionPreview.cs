using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor
{
    public class InstructionPreview : Preview<InstructionInspector>
    {
        /* Public properties. */
        public List<ParameterPreview> Parameters { get; } = new();
        public List<CompileRulePreview> CompileRules { get; } = new();

        /* Constructors. */
        public InstructionPreview(InstructionInspector inspector)
            : base(inspector, inspector, inspector.Definition.Preview)
        {
            // Create parameter previews.
            for (int i = 0; i < Inspector.Definition.Parameters.Length; i++)
            {
                ParameterInspector parameter = Inspector.GetParameterInspector(i);
                Parameters.Add(new(this, parameter));
            }

            // Create compile rule previews.
            for (int i = 0; i < Inspector.Definition.PreInstructions.Length; i++)
            {
                // TODO
            }
            for (int i = 0; i < Inspector.Definition.PostInstructions.Length; i++)
            {
                // TODO
            }


        }

        /* Protected methods. */
        protected override string GetDefaultExpression()
        {
            return "";
        }

        protected override string ParseParameter(string parameterID)
        {
            int index = Inspector.Definition.GetParameterIndex(parameterID);
            if (index >= 0)
                return $"\"{Parameters[index].Evaluate()}\"";
            return GetParameterError(parameterID);
        }

        protected override string ParseCompileRule(string ruleID)
        {
            return "";
        }
    }
}
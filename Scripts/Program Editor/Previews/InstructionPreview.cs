using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor
{
    public class InstructionPreview : Preview<InstructionInspector>
    {
        /* Public properties. */
        public List<ParameterPreview> Parameters { get; } = new();
        public List<CompileRulePreview> PreInstructions { get; } = new();
        public List<CompileRulePreview> PostInstructions { get; } = new();

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
                CompileRuleInspector rule = Inspector.PreInstructions.Inspectors[i];
                PreInstructions.Add(new(this, rule));
            }
            for (int i = 0; i < Inspector.Definition.PostInstructions.Length; i++)
            {
                CompileRuleInspector rule = Inspector.PostInstructions.Inspectors[i];
                PostInstructions.Add(new(this, rule));
            }
        }

        /* Public methods. */
        /// <summary>
        /// Parse a parameter of an instruction preview and return it as an expression.
        /// </summary>
        public static string ParseParameter(InstructionPreview parent, string parameterID)
        {
            int index = parent.Inspector.Definition.GetParameterIndex(parameterID);
            if (index >= 0)
                return Make(parent.Parameters[index].Evaluate());
            return GetParameterError(parameterID);
        }

        /// <summary>
        /// Parse a compile rule of an instruction preview and return it as an expression.
        /// </summary>
        public static string ParseCompileRule(InstructionPreview parent, string ruleID)
        {
            for (int i = 0; i < parent.PreInstructions.Count; i++)
            {
                if (parent.PreInstructions[i].Inspector.Definition.ID == ruleID)
                    return Make(parent.PreInstructions[i].Evaluate());
            }
            for (int i = 0; i < parent.PostInstructions.Count; i++)
            {
                if (parent.PostInstructions[i].Inspector.Definition.ID == ruleID)
                    return Make(parent.PostInstructions[i].Evaluate());
            }
            return GetRuleError(ruleID);
        }

        /* Protected methods. */
        protected override string GetDebugName()
        {
            return "InstructionDefinition " + Inspector.Definition.Opcode;
        }

        protected override string GetDefaultExpression()
        {
            return "";
        }

        protected override string ParseParameter(string parameterID)
        {
            return ParseParameter(this, parameterID);
        }

        protected override string ParseCompileRule(string ruleID)
        {
            return ParseCompileRule(this, ruleID);
        }
    }
}
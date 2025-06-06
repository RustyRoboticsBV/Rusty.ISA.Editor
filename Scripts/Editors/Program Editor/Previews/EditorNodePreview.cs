﻿using Godot;

namespace Rusty.ISA.Editor.Programs
{
    public class EditorNodePreview : Preview<NodeInstructionInspector>
    {
        /* Constructors. */
        public EditorNodePreview() : base() { }

        public EditorNodePreview(NodeInstructionInspector inspector)
            : base(inspector, inspector.Definition.EditorNode.Preview) { }

        /* Protected methods. */
        protected override string GetDebugName()
        {
            if (Inspector != null)
                return "EditorNode " + Inspector.Definition.Opcode;
            else
                return "EditorNode null";
        }

        protected override string GetDefaultExpression()
        {
            return Make((Inspector as InstructionInspector).Preview.Evaluate());
        }

        protected override string[] GetSpecialKeywords()
        {
            return new string[] { "<base>" };
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

        protected override string ParseSpecialKeyword(string keyword)
        {
            if (keyword == "<base>")
                return Make((Inspector as InstructionInspector).Preview.Evaluate());
            else
                return GetSpecialKeywordError(keyword);
        }
    }
}
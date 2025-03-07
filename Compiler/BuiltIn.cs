namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// Built-in opcodes and parameter ids.
    /// </summary>
    public static class BuiltIn
    {
        /* Constants. */
        // Opcodes.
        public const string BeginOpcode = "BEG";
        public const string LabelOpcode = "LAB";
        public const string GotoOpcode = "GTO";
        public const string WarningOpcode = "WRN";
        public const string ErrorOpcode = "ERR";
        public const string EndOpcode = "END";

        public const string CommentOpcode = "CMT";
        public const string FrameOpcode = "FRM";
        public const string NodeOpcode = "NOD";
        public const string PreInstructionOpcode = "PRE";
        public const string PostInstructionOpcode = "PST";
        public const string OptionRuleOpcode = "OPT";
        public const string ChoiceRuleOpcode = "CHO";
        public const string TupleRuleOpcode = "TPL";
        public const string ListRuleOpcode = "LST";
        public const string EndOfGroupOpcode = "EOG";

        // Parameter ids.
        public const string StartNameId = "name";
        public const string LabelNameId = "name";
        public const string GoToTargetLabelId = "target_label";
        public const string ErrorMessageId = "message";

        public const string CommentTextId = "text";
        public const string FrameNameId = "name";
        public const string NodeXId = "x";
        public const string NodeYId = "y";
        public const string OptionRuleEnabledId = "enabled";
        public const string ChoiceRuleSelectedId = "selected";
        public const string TupleRuleLengthId = "length";
        public const string ListRuleCountId = "count";
    }
}
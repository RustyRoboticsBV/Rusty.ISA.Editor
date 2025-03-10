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
        public const string InspectorOpcode = "INS";
        public const string PreInstructionOpcode = "PRE";
        public const string PostInstructionOpcode = "PST";
        public const string OptionRuleOpcode = "OPT";
        public const string ChoiceRuleOpcode = "CHO";
        public const string TupleRuleOpcode = "TPL";
        public const string ListRuleOpcode = "LST";
        public const string EndOfGroupOpcode = "EOG";

        // Parameter ids.
        public const string BeginNameID = "name";
        public const string LabelNameID = "name";
        public const string GotoTargetLabelID = "target_label";
        public const string ErrorMessageID = "message";

        public const string CommentTextID = "text";
        public const string FrameNameID = "name";
        public const string NodeXID = "x";
        public const string NodeYID = "y";
        public const string OptionRuleEnabledID = "enabled";
        public const string ChoiceRuleSelectedID = "selected";
        public const string TupleRuleLengthID = "length";
        public const string ListRuleCountID = "count";
    }
}
namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// Built-in opcodes and parameter ids.
    /// </summary>
    public static class BuiltIn
    {
        /* Constants. */
        // Opcodes.
        public const string LabelOpcode = "LBL";
        public const string GoToOpcode = "GTO";
        public const string ErrorOpcode = "ERR";
        public const string EndOpcode = "END";

        public const string CommentOpcode = "CMT";
        public const string NodeOpcode = "NOD";
        public const string FrameOpcode = "FRM";
        public const string OptionRuleOpcode = "OPT";
        public const string ChoiceRuleOpcode = "CHO";
        public const string TupleRuleOpcode = "TPL";
        public const string ListRuleOpcode = "LST";
        public const string EndOfRuleOpcode = "EOR";

        // Parameter ids.
        public const string LabelNameId = "name";
        public const string GoToTargetLabelId = "target_label";
        public const string ErrorMessageId = "message";

        public const string CommentTextId = "text";
        public const string NodeXId = "x";
        public const string NodeYId = "y";
        public const string FrameNameId = "name";
        public const string OptionRuleEnabledId = "enabled";
        public const string ChoiceRuleSelectedId = "selected";
        public const string TupleRuleLengthId = "length";
        public const string ListRuleCountId = "count";
    }
}
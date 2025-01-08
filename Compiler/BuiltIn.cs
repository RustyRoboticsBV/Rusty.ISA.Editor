namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// Built-in opcodes and parameter ids.
    /// </summary>
    public static class BuiltIn
    {
        /* Constants. */
        // Opcodes.
        public const string StartOpcode = "STA";
        public const string LabelOpcode = "LBL";
        public const string GotoOpcode = "GTO";
        public const string ErrorOpcode = "ERR";
        public const string EndOpcode = "END";

        public const string CommentOpcode = "CMT";
        public const string FrameOpcode = "FRM";
        public const string NodeOpcode = "NOD";
        public const string PreInstructionGroupOpcode = "PRE";
        public const string OptionRuleOpcode = "OPT";
        public const string ChoiceRuleOpcode = "CHO";
        public const string TupleRuleOpcode = "TPL";
        public const string ListRuleOpcode = "LST";
        public const string EndOfBlockOpcode = "EOB";

        // Parameter ids.
        public const string StartNameId = "name";
        public const string LabelNameId = "name";
        public const string GoToTargetLabelId = "target_label";
        public const string ErrorMessageId = "message";

        public const string CommentTextId = "text";
        public const string FrameNameId = "name";
        public const string NodeXId = "x";
        public const string NodeYId = "y";
    }
}
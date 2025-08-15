namespace Rusty.ISA.Editor;

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

    public const string FileOpcode = "FIL";

    public const string MetadataOpcode = "MTA";
    public const string ChecksumOpcode = "MD5";
    public const string InstructionSetOpcode = "ISA";
    public const string DefinitionOpcode = "DEF";
    public const string ParameterOpcode = "PAR";
    public const string CompileRuleOpcode = "RUL";
    public const string ReferenceOpcode = "REF";

    public const string ProgramOpcode = "PRO";
    public const string JointOpcode = "JNT";
    public const string CommentOpcode = "CMT";
    public const string FrameOpcode = "FRM";
    public const string FrameMemberOpcode = "MBR";
    public const string NodeOpcode = "NOD";
    public const string InspectorOpcode = "INS";
    public const string PreInstructionOpcode = "PRE";
    public const string PostInstructionOpcode = "PST";
    public const string OptionRuleOpcode = "OPT";
    public const string ChoiceRuleOpcode = "CHO";
    public const string TupleRuleOpcode = "TPL";
    public const string ListRuleOpcode = "LST";
    public const string GotoGroupOpcode = "GTG";
    public const string EndGroupOpcode = "ENG";
    public const string EndOfGroupOpcode = "EOG";

    // Parameter ids.
    public const string BeginName = "name";
    public const string LabelName = "name";
    public const string GotoTargetLabel = "target_label";
    public const string ErrorMessage = "message";

    public const string JointX = "x";
    public const string JointY = "y";
    public const string CommentText = "text";
    public const string CommentX = "x";
    public const string CommentY = "y";
    public const string FrameID = "id";
    public const string FrameX = "x";
    public const string FrameY = "y";
    public const string FrameWidth = "width";
    public const string FrameHeight = "height";
    public const string FrameTitle = "title";
    public const string FrameColor = "color_tint";
    public const string FrameMemberID = "frame_id";
    public const string NodeX = "x";
    public const string NodeY = "y";
    public const string ChoiceRuleSelected = "selected";

    public const string ChecksumValue = "value";
    public const string DefinitionOpcodeParameter = "opcode";
    public const string ParameterType = "type";
    public const string ParameterID = "id";
    public const string CompileRuleType = "type";
    public const string CompileRuleID = "id";
    public const string ReferenceOpcodeParameter = "opcode";
    public const string ReferenceID = "id";
}
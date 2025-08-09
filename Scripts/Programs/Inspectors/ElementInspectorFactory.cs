namespace Rusty.ISA.Editor;

public static class ElementInspectorFactory
{
    /* Constants. */
    public const string StartPoint = "start_point";
    public const string Instruction = "instruction";
    public const string Parameter = InstructionInspectorFactory.Parameter;

    /* Public methods. */
    public static Inspector Create(InstructionSet set, InstructionDefinition definition)
    {
        // Create inspector.
        Inspector inspector = new();

        // Change contents container.
        FoldoutBorderContainer foldout = new();
        foldout.FoldoutText = definition.DisplayName;
        foldout.IsOpen = true;
        inspector.ReplaceContainer(foldout);

        // Add title.
        LabeledIcon title = new();
        title.LabelText = definition.DisplayName;
        title.Texture = definition.Icon;
        inspector.Add("icon+name", title);

        // Create instruction inspector.
        switch (definition.Opcode)
        {
            case BuiltIn.JointOpcode:
                break;

            case BuiltIn.CommentOpcode:
                // Add text field.
                MultilineParameter commentTextParam = definition.GetParameter(BuiltIn.CommentText) as MultilineParameter;
                MultilineField commentText = new();
                commentText.LabelText = commentTextParam.DisplayName;
                commentText.Value = commentTextParam.DefaultValue;
                commentText.TooltipText = commentTextParam.Description;
                inspector.Add(Parameter + BuiltIn.CommentText, commentText);
                break;

            case BuiltIn.FrameOpcode:
                // Add title field.
                TextlineParameter frameTitleParam = definition.GetParameter(BuiltIn.FrameTitle) as TextlineParameter;
                LineField frameTitleField = new();
                frameTitleField.LabelText = frameTitleParam.DisplayName;
                frameTitleField.Value = frameTitleParam.DefaultValue;
                frameTitleField.TooltipText = frameTitleParam.Description;
                inspector.Add(Parameter + BuiltIn.FrameTitle, frameTitleField);

                // Add color field.
                ColorParameter frameColorParam = definition.GetParameter(BuiltIn.FrameColor) as ColorParameter;
                ColorField frameColorField = new();
                frameColorField.LabelText = frameColorParam.DisplayName;
                frameColorField.Value = frameColorParam.DefaultValue;
                frameColorField.TooltipText = frameColorParam.Description;
                inspector.Add(Parameter + BuiltIn.FrameColor, frameColorField);
                break;

            default:
                // Add start point checkbox.
                ToggleTextField startPoint = new();
                startPoint.LabelText = "Start Point";
                startPoint.FieldText = "Start";
                startPoint.TooltipText = "Defines whether or not this node is a start point from which this program can be ran.";
                inspector.Add(StartPoint, startPoint);

                // Add instruction inspector.
                Inspector instructionInspector = InstructionInspectorFactory.Create(set, definition);
                inspector.Add(Instruction, instructionInspector);
                break;
        }

        return inspector;
    }
}
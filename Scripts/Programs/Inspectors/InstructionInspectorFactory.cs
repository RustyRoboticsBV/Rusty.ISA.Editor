namespace Rusty.ISA.Editor;

/// <summary>
/// A factory for instruction inspectors.
/// </summary>
public static class InstructionInspectorFactory
{
    /* Constants. */
    public const string Opcode = "opcode";
    public const string Parameter = "par_";
    public const string PreInstruction = "pre_";
    public const string PostInstruction = "pst_";

    /* Public methods. */
    public static Inspector Create(InstructionSet set, InstructionDefinition definition)
    {
        // Create inspector.
        Inspector inspector = new();

        // Store opcode.
        inspector.WriteMetaData(Opcode, definition.Opcode);

        // Add pre-instructions.
        foreach (CompileRule rule in definition.PreInstructions)
        {
            IGuiElement element = CompileRuleInspectorFactory.Create(set, rule);
            if (element != null)
                inspector.Add(PreInstruction + rule.ID, element);
        }

        // Add parameters.
        foreach (Parameter parameter in definition.Parameters)
        {
            IGuiElement element = CreateField(parameter);
            if (element != null)
                inspector.Add(Parameter + parameter.ID, element);
        }

        // Add post-instructions.
        foreach (CompileRule rule in definition.PostInstructions)
        {
            IGuiElement element = CompileRuleInspectorFactory.Create(set, rule);
            if (element != null)
                inspector.Add(PostInstruction + rule.ID, element);
        }

        return inspector;
    }

    /* Private methods. */
    private static IGuiElement CreateField(Parameter parameter)
    {
        IGuiElement element;
        switch (parameter)
        {
            case BoolParameter b:
                element = new BoolField()
                {
                    Value = b.DefaultValue
                };
                break;
            case IntParameter i:
                element = new IntField()
                {
                    Value = i.DefaultValue
                };
                break;
            case IntSliderParameter isl:
                element = new IntSliderField()
                {
                    Value = isl.DefaultValue,
                    MinValue = isl.MinValue,
                    MaxValue = isl.MaxValue
                };
                break;
            case FloatParameter f:
                element = new FloatField()
                {
                    Value = f.DefaultValue
                };
                break;
            case FloatSliderParameter fsl:
                element = new FloatSliderField()
                {
                    Value = fsl.DefaultValue,
                    MinValue = fsl.MinValue,
                    MaxValue = fsl.MaxValue
                };
                break;
            case CharParameter c:
                element = new CharField()
                {
                    Value = c.DefaultValue
                };
                break;
            case TextlineParameter l:
                element = new LineField()
                {
                    Value = l.DefaultValue
                };
                break;
            case MultilineParameter ml:
                element = new MultilineField()
                {
                    Value = ml.DefaultValue
                };
                break;
            case ColorParameter col:
                element = new ColorField()
                {
                    Value = col.DefaultValue
                };
                break;
            default:
                element = null;
                break;
        }

        if (element is IField field)
        {
            field.LabelText = parameter.DisplayName;
            field.TooltipText = parameter.Description;
        }

        return element;
    }
}
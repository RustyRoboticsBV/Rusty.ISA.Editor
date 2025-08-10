namespace Rusty.ISA.Editor;

/// <summary>
/// A factory for parameter fields.
/// </summary>
public static class ParameterFieldFactory
{
    /* Public methods. */
    public static IGuiElement Create(InstructionDefinition definition, string parameterID)
    {
        return Create(definition.Parameters[definition.GetParameterIndex(parameterID)]);
    }

    public static IGuiElement Create(Parameter parameter)
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
            case OutputParameter o:
                element = new OutputField();
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
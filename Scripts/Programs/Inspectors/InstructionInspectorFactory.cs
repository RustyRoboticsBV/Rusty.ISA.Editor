namespace Rusty.ISA.Editor;

public static class InstructionInspectorFactory
{
    public static Inspector Create(InstructionSet set, InstructionDefinition definition)
    {
        // Create inspector.
        Inspector inspector = new();

        // Add parameters.
        foreach (Parameter parameter in definition.Parameters)
        {
            Godot.GD.Print(parameter);
            IGuiElement element = CreateField(parameter);
            if (element != null)
                inspector.Add("par_" + parameter.ID, element);
        }

        // Add pre-instructions.
        foreach (CompileRule rule in definition.PreInstructions)
        {
            IGuiElement element = CompileRuleInspectorFactory.Create(set, rule);
            if (element != null)
                inspector.Add("pre_" + rule.ID, element);
        }

        // Add post-instructions.
        foreach (CompileRule rule in definition.PostInstructions)
        {
            IGuiElement element = CompileRuleInspectorFactory.Create(set, rule);
            if (element != null)
                inspector.Add("pst_" + rule.ID, element);
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
                    LabelText = b.DisplayName,
                    Value = b.DefaultValue
                };
                break;
            case IntParameter i:
                element = new IntField()
                {
                    LabelText = i.DisplayName,
                    Value = i.DefaultValue
                };
                break;
            case IntSliderParameter isl:
                element = new IntSliderField()
                {
                    LabelText = isl.DisplayName,
                    Value = isl.DefaultValue,
                    MinValue = isl.MinValue,
                    MaxValue = isl.MaxValue
                };
                break;
            case FloatParameter f:
                element = new FloatField()
                {
                    LabelText = f.DisplayName,
                    Value = f.DefaultValue
                };
                break;
            case FloatSliderParameter fsl:
                element = new FloatSliderField()
                {
                    LabelText = fsl.DisplayName,
                    Value = fsl.DefaultValue,
                    MinValue = fsl.MinValue,
                    MaxValue = fsl.MaxValue
                };
                break;
            case CharParameter c:
                element = new CharField()
                {
                    LabelText = c.DisplayName,
                    Value = c.DefaultValue
                };
                break;
            case TextlineParameter l:
                element = new LineField()
                {
                    LabelText = l.DisplayName,
                    Value = l.DefaultValue
                };
                break;
            case MultilineParameter ml:
                element = new MultilineField()
                {
                    LabelText = ml.DisplayName,
                    Value = ml.DefaultValue
                };
                break;
            case ColorParameter col:
                element = new ColorField()
                {
                    LabelText = col.DisplayName,
                    Value = col.DefaultValue
                };
                break;
            default:
                element = null;
                break;
        }

        return element;
    }
}
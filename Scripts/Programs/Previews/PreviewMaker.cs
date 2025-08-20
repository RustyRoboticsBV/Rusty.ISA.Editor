using Godot;

namespace Rusty.ISA.Editor;

public static class PreviewMaker
{
    /* Private constants. */
    private const string Name = "name";
    private const string Value = "value";
    private const string Main = "main";
    private const string Element = "element";
    private const string Enabled = "enabled";
    private const string Selected = "selected";
    private const string Count = "count";

    private const string DefaultInstruction = "";
    private const string DefaultNode = $"[[{Main}]]";
    private const string DefaultParameter = $"return [[{Value}]];";
    private const string DefaultOutput = $"return \"[[{Name}]]\";";
    private const string DefaultRule = $"[[{Element}]]";
    private const string DefaultCollection =
            $"var result : String = \"\";"
            + $"\nfor i in [[{Count}]]:"
                + $"\n\tif i > 0:"
                    + $"\n\t\tresult += '\\n'"
                + $"\n\tresult += [[{Element}i]];"
            + $"\nreturn result;";

    /* Public methods. */
    public static PreviewInstance GraphNode(NodeInspector inspector)
    {
        InstructionInspector instructionInspector = inspector.GetInstructionInspector();
        EditorNodeInfo editorNodeInfo = instructionInspector.Definition.EditorNode;

        // Get input.
        PreviewInput input = GetInstructionInput(instructionInspector);
        input.AddValue(Main, InstructionInspector(instructionInspector));

        // Get instance of preview.
        return GetPreview(editorNodeInfo, editorNodeInfo.Preview, DefaultNode);
    }

    public static PreviewInstance InstructionInspector(InstructionInspector inspector)
    {
        InstructionDefinition definition = inspector.Definition;

        // Create input.
        PreviewInput input = GetInstructionInput(inspector);

        // Get instance of preview.
        return GetPreview(definition, definition.Preview, DefaultInstruction);
    }

    public static ParameterPreviewInstance FromParameter(Parameter parameter, IField field)
    {
        // Add to preview dict if necessary.
        if (!PreviewDict.Has(parameter))
            PreviewDict.Add(parameter, new ParameterPreview(parameter));

        // Create instance.
        var preview = PreviewDict.CreateInstance(parameter) as ParameterPreviewInstance;
        preview.SetDisplayName(parameter.DisplayName);
        preview.SetValue(field.Value);
        return preview;
    }

    public static PreviewInstance FromOutputParameter(OutputParameter output, InstructionInspector inspector)
    {
        // Create input.
        PreviewInput input = new();
        input.AddValue(Name, output.DisplayName);
        for (int i = 0; i < inspector.Definition.Parameters.Length; i++)
        {
            Parameter parameter = inspector.Definition.Parameters[i];
            if (parameter is not OutputParameter)
            {
                PreviewInstance preview = FromParameter(parameter, inspector.GetParameterField(parameter.ID));
                input.AddValue(parameter.ID, preview);
            }
        }

        // Get instance of preview.
        return GetPreview(output, output.Preview, DefaultOutput);
    }

    public static PreviewInstance CompileRule(RuleInspector inspector)
    {
        switch (inspector)
        {
            case InstructionRuleInspector i:
                return InstructionRule(i);
            case OptionRuleInspector o:
                return OptionRule(o);
            case ChoiceRuleInspector c:
                return ChoiceRule(c);
            case TupleRuleInspector t:
                return TupleRule(t);
            case ListRuleInspector l:
                return ListRule(l);
            default:
                return null;
        }
    }

    public static PreviewInstance InstructionRule(InstructionRuleInspector inspector)
    {
        // Create input.
        PreviewInput input = new();
        input.AddValue(Name, inspector.Rule.DisplayName);
        input.AddValue(Element, inspector.GetInstructionInspector());

        // Get instance of preview.
        return GetPreview(inspector.Rule, inspector.Rule.Preview, DefaultRule);
    }

    public static PreviewInstance OptionRule(OptionRuleInspector inspector)
    {
        CompileRule rule = inspector.Rule;

        // Create input.
        PreviewInput input = new();
        input.AddValue(Name, inspector.Rule.DisplayName);
        input.AddValue(Enabled, inspector.GetEnabled());
        input.AddValue(Element, CompileRule(inspector.GetChildRule()).Evaluate());

        // Get instance of preview.
        return GetPreview(inspector.Rule, inspector.Rule.Preview, DefaultRule);
    }

    public static PreviewInstance ChoiceRule(ChoiceRuleInspector inspector)
    {
        // Create input.
        PreviewInput input = new();
        input.AddValue(Name, inspector.Rule.DisplayName);
        input.AddValue(Selected, inspector.GetSelectedIndex());
        input.AddValue(Element, CompileRule(inspector.GetSelectedElement()).Evaluate());

        // Create preview.
        return GetPreview(inspector.Rule, inspector.Rule.Preview, DefaultRule);
    }

    public static PreviewInstance TupleRule(TupleRuleInspector inspector)
    {
        // Create input.
        PreviewInput input = new();
        input.AddValue(Name, inspector.Rule.DisplayName);
        for (int i = 0; i < inspector.GetElementCount(); i++)
        {
            var childInspector = inspector.GetElementInspector(i);
            PreviewInstance value = CompileRule(childInspector);
            input.AddValue("element" + i, value);
            input.AddValue(childInspector.Rule.ID, value);
        }
        input.AddValue(Count, inspector.GetElementCount().ToString());

        // Create preview.
        return GetPreview(inspector.Rule, inspector.Rule.Preview, DefaultCollection);
    }

    public static PreviewInstance ListRule(ListRuleInspector inspector)
    {
        // Create input.
        PreviewInput input = new();
        input.AddValue(Name, inspector.Rule.DisplayName);
        for (int i = 0; i < inspector.GetElementCount(); i++)
        {
            input.AddValue("element" + i, CompileRule(inspector.GetElementInspector(i)));
        }
        input.AddValue(Count, inspector.GetElementCount());

        // Create preview.
        return GetPreview(inspector.Rule, inspector.Rule.Preview, DefaultCollection);
    }

    /* Private methods. */
    private static string Stringify(object value)
    {
        if (value is Color color)
            return '#' + color.ToHtml(color.A < 1f);
        else
            return value.ToString();
    }
    private string MakeIdSafe(string id)
    {
        switch (id)
        {
            case 
        }
    }

    /// <summary>
    /// Get a preview instance for some resource, which creates it if needed.
    /// </summary>
    private static PreviewInstance GetPreview(InstructionResource resource, string code, string @default)
    {
        // Create new preview if it wasn't added yet.
        if (!PreviewDict.Has(resource))
        {
            Preview preview = null;
            if (string.IsNullOrEmpty(code))
                preview = new(resource.ResourceName, @default);
            else
                preview = new(resource.ResourceName, code);
            PreviewDict.Add(resource, preview);
        }

        // Create instance of preview.
        return PreviewDict.CreateInstance(resource);
    }

    /// <summary>
    /// Get the input of an instruction.
    /// </summary>
    private static PreviewInput GetInstructionInput(InstructionInspector inspector)
    {
        PreviewInput input = new();
        InstructionDefinition definition = inspector.Definition;

        // Add display name value.
        input.AddValue(Name, definition.DisplayName);

        // Add parameter previews.
        for (int i = 0; i < definition.Parameters.Length; i++)
        {
            Parameter parameter = definition.Parameters[i];
            if (parameter is OutputParameter output)
            {
                input.AddValue(parameter.ID, FromOutputParameter(output, inspector));
            }
            else
            {
                IField field = inspector.GetParameterField(parameter.ID);
                input.AddValue(parameter.ID, FromParameter(parameter, field));
            }
        }

        // Add compile rule previews.
        for (int i = 0; i < definition.PreInstructions.Length; i++)
        {
            CompileRule rule = definition.PreInstructions[i];
            input.AddValue(rule.ID, CompileRule(inspector.GetPreInstruction(rule.ID)));
        }
        for (int i = 0; i < definition.PostInstructions.Length; i++)
        {
            CompileRule rule = definition.PostInstructions[i];
            input.AddValue(rule.ID, CompileRule(inspector.GetPostInstruction(rule.ID)));
        }

        return input;
    }
}
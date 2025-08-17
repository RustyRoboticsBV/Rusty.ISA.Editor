using Godot;

namespace Rusty.ISA.Editor;

public static class PreviewMaker
{
    private const string This = "this";
    private const string Base = "base";
    private const string Element = "element";
    private const string Count = "count";

    public static Preview Parameter(Parameter parameter, IField field)
    {
        // Create input.
        PreviewInput input = new();
        input.AddValue(This, Stringify(field.Value));

        // Create preview.
        string code = parameter.Preview;
        if (code == "")
            code = $"return [[this]];";
        return new Preview(parameter.ID, code, input);
    }

    public static Preview Output(OutputParameter output, InstructionInspector inspector)
    {
        // Create input.
        PreviewInput input = new();
        for (int i = 0; i < inspector.Definition.Parameters.Length; i++)
        {
            Parameter parameter = inspector.Definition.Parameters[i];
            if (parameter is not OutputParameter)
            {
                IField field = inspector.GetParameterField(parameter.ID);
                input.AddValue(parameter.ID, Parameter(parameter, field).Evaluate());
            }
        }

        // Create preview.
        string code = output.Preview;
        if (code == "")
            code = $"return \"{output.DisplayName}\";";
        return new Preview(output.ID, code, input);
    }

    public static Preview InstructionInspector(InstructionInspector inspector)
    {
        InstructionDefinition definition = inspector.Definition;

        // Create input.
        PreviewInput input = new();
        for (int i = 0; i < definition.Parameters.Length; i++)
        {
            Parameter parameter = definition.Parameters[i];
            if (parameter is OutputParameter output)
            {
                input.AddValue(parameter.ID, Output(output, inspector).Evaluate());
            }
            else
            {
                IField field = inspector.GetParameterField(parameter.ID);
                input.AddValue(parameter.ID, Parameter(parameter, field).Evaluate());
            }
        }
        for (int i = 0; i < definition.PreInstructions.Length; i++)
        {
            CompileRule rule = definition.PreInstructions[i];
            input.AddValue(rule.ID, RuleInspector(inspector.GetPreInstruction(rule.ID)).Evaluate());
        }
        for (int i = 0; i < definition.PostInstructions.Length; i++)
        {
            CompileRule rule = definition.PostInstructions[i];
            input.AddValue(rule.ID, RuleInspector(inspector.GetPostInstruction(rule.ID)).Evaluate());
        }

        // Create preview.
        return new Preview(definition.Opcode, definition.Preview, input);
    }

    public static Preview GraphNode(NodeInspector inspector)
    {
        InstructionInspector instructionInspector = inspector.GetInstructionInspector();

        // Create input.
        PreviewInput input = new();
        input.AddValue(Base, InstructionInspector(instructionInspector).Evaluate());

        // Create preview.
        string name = "node " + instructionInspector.Definition.Opcode;
        string code = instructionInspector.Definition.EditorNode?.Preview;
        if (code == "")
            code = $"return [[{Base}]];";
        return new Preview(name, code, input);
    }

    public static Preview InstructionRuleInspector(InstructionRuleInspector inspector)
    {
        string code = inspector.Rule.Preview;
        return InstructionInspector(inspector.GetInstructionInspector());
    }

    public static Preview RuleInspector(RuleInspector inspector)
    {
        switch (inspector)
        {
            case InstructionRuleInspector i:
                return InstructionRuleInspector(i);
            case OptionRuleInspector o:
                return OptionRuleInspector(o);
            case ChoiceRuleInspector c:
                return ChoiceRuleInspector(c);
            case TupleRuleInspector t:
                return TupleRuleInspector(t);
            case ListRuleInspector l:
                return ListRuleInspector(l);
            default:
                return null;
        }
    }

    public static Preview OptionRuleInspector(OptionRuleInspector inspector)
    {
        CompileRule rule = inspector.Rule;

        // Create input.
        PreviewInput input = new();
        if (inspector.GetEnabled())
            input.AddValue(Element, RuleInspector(inspector.GetChildRule()).Evaluate());

        // Create preview.
        string code = rule.Preview;
        if (code == "" && inspector.GetEnabled())
            code = $"return [[{Element}]];";
        return new Preview(rule.ID, code, input);
    }

    public static Preview ChoiceRuleInspector(ChoiceRuleInspector inspector)
    {
        CompileRule rule = inspector.Rule;

        // Create input.
        PreviewInput input = new();
        input.AddValue(Element, RuleInspector(inspector.GetSelectedElement()).Evaluate());

        // Create preview.
        string code = rule.Preview;
        if (code == "")
            code = $"return [[{Element}]];";
        return new Preview(rule.ID, code, input);
    }

    public static Preview TupleRuleInspector(TupleRuleInspector inspector)
    {
        CompileRule rule = inspector.Rule;

        // Create input.
        PreviewInput input = new();
        for (int i = 0; i < inspector.GetElementCount(); i++)
        {
            var childInspector = inspector.GetElementInspector(i);
            string value = RuleInspector(childInspector).Evaluate();
            input.AddValue(i.ToString(), value);
            input.AddValue(childInspector.Rule.ID, value);
        }
        input.AddValue(Count, inspector.GetElementCount().ToString());

        // Create preview.
        string code = rule.Preview;
        if (code == "")
            code = $"var result : String = \"\";\nfor i in [[{Count}]]:\n\tresult += '\n' + [[{Element}i]];\nreturn result;";
        return new Preview(rule.ID, code, input);
    }

    public static Preview ListRuleInspector(ListRuleInspector inspector)
    {
        CompileRule rule = inspector.Rule;

        // Create input.
        PreviewInput input = new();
        for (int i = 0; i < inspector.GetElementCount(); i++)
        {
            string value = RuleInspector(inspector.GetElementInspector(i)).Evaluate();
            input.AddValue(i.ToString(), value);
        }
        input.AddValue(Count, inspector.GetElementCount());

        // Create preview.
        string code = rule.Preview;
        if (code == "")
            code = $"var result : String = \"\";\nfor i in [[{Count}]]:\n\tif i > 0:\n\t\tresult += '\\n'\n\tresult += [[{Element}i]];\nreturn result;";
        return new Preview(rule.ID, code, input);
    }

    /* Private methods. */
    private static string Stringify(object value)
    {
        if (value is Color color)
            return '#' + color.ToHtml(color.A < 1f);
        else
            return value.ToString();
    }
}
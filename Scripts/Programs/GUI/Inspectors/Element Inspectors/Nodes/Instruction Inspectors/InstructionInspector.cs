namespace Rusty.ISA.Editor;

public partial class InstructionInspector : ResourceInspector
{
    /* Constants. */
    public const string Parameter = "par_";
    public const string PreInstruction = "pre_";
    public const string PostInstruction = "pst_";

    /* Public properties. */
    public InstructionDefinition Definition { get; private set; }
    public new InstructionPreviewInstance Preview
    {
        get => base.Preview as InstructionPreviewInstance;
        private set => base.Preview = value;
    }

    /* Constructors. */
    public InstructionInspector(InstructionSet set, string opcode) : base(set)
    {
        // Store data.
        Definition = set[opcode];

        // Add pre-instructions.
        foreach (CompileRule rule in Definition.PreInstructions)
        {
            RuleInspector element = RuleInspectorFactory.Create(set, rule);
            if (element != null)
                AddPreInstruction(rule.ID, element);
        }

        // Add parameters.
        foreach (Parameter parameter in Definition.Parameters)
        {
            if (parameter is OutputParameter output)
                AddParameter(output.ID, new OutputInspector(InstructionSet, Definition.Opcode, output.ID));
            else
                AddParameter(parameter.ID, new ParameterInspector(InstructionSet, Definition.Opcode, parameter.ID));
        }

        // Add post-instructions.
        foreach (CompileRule rule in Definition.PostInstructions)
        {
            RuleInspector element = RuleInspectorFactory.Create(set, rule);
            if (element != null)
                AddPreInstruction(rule.ID, element);
        }

        // Enable preview.
        EnablePreview();
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        InstructionInspector copy = new(InstructionSet, Definition.Opcode);
        copy.CopyFrom(this);
        return copy;
    }

    public override void CopyFrom(IGuiElement other)
    {
        DisablePreview();

        // Base resource inspector copy.
        base.CopyFrom(other);

        // Copy definition.
        if (other is InstructionInspector inspector)
            Definition = inspector.Definition;

        EnablePreview();
    }

    public RuleInspector GetPreInstruction(string id)
    {
        if (GetAt(PreInstruction + id) is RuleInspector inspector)
            return inspector;
        return null;
    }

    public ParameterInspector GetParameterInspector(string id)
    {
        if (GetAt(Parameter + id) is ParameterInspector parameter)
            return parameter;
        return null;
    }

    public void SetParameterField(string id, string value)
    {
        GetParameterInspector(id)?.ParseValue(value);
    }

    public RuleInspector GetPostInstruction(string id)
    {
        if (GetAt(PostInstruction + id) is RuleInspector inspector)
            return inspector;
        return null;
    }

    /* Protected methods. */
    protected override void UpdatePreview()
    {
        // Init preview.
        if (Preview == null)
            Preview = PreviewDict.ForInstruction(InstructionSet, Definition)?.CreateInstance();

        // Update preview.
        if (Preview != null)
        {
            Preview.SetDisplayName(Definition.DisplayName);

            for (int i = 0; i < GetContentsCount(); i++)
            {
                switch (GetAt(i))
                {
                    case ParameterInspector parameter:
                        if (parameter is OutputInspector output)
                            output.UpdatePreviewParameterValues(this);
                        Preview.SetParameter(parameter.Parameter.ID, parameter.Preview);
                        break;

                    case RuleInspector rule:
                        Preview.SetRule(rule.Rule.ID, rule.Preview);
                        break;
                }
            }
        }
    }

    /* Private methods. */
    private void AddParameter(string key, ParameterInspector inspector)
    {
        Add(Parameter + key, inspector);

        Preview?.SetParameter(key, inspector.Preview);
    }

    private void AddPreInstruction(string key, RuleInspector inspector)
    {
        Add(PreInstruction + key, inspector);

        Preview?.SetRule(key, inspector.Preview);
    }

    private void AddPostInstruction(string key, RuleInspector inspector)
    {
        Add(PostInstruction + key, inspector);

        Preview?.SetRule(key, inspector.Preview);
    }
}
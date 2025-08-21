using Godot;

namespace Rusty.ISA.Editor;

public partial class InstructionInspector : Inspector
{
    /* Constants. */
    public const string Parameter = "par_";
    public const string PreInstruction = "pre_";
    public const string PostInstruction = "pst_";

    /* Public properties. */
    public InstructionDefinition Definition { get; private set; }
    public InstructionPreviewInstance Preview { get; private set; }

    /* Constructors. */
    public InstructionInspector() : base() { }

    public InstructionInspector(InstructionSet set, string opcode) : base()
    {
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
                AddParameter(output.ID, new OutputInspector(this, output));
            else
                AddParameter(parameter.ID, new ParameterInspector(parameter));
        }

        // Add post-instructions.
        foreach (CompileRule rule in Definition.PostInstructions)
        {
            RuleInspector element = RuleInspectorFactory.Create(set, rule);
            if (element != null)
                AddPreInstruction(rule.ID, element);
        }

        Preview = PreviewDict.ForInstruction(set, Definition)?.CreateInstance();
        Changed += UpdateOutputPreviews;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        InstructionInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public override void CopyFrom(IGuiElement other)
    {
        base.CopyFrom(other);
        if (other is InstructionInspector inspector)
        {
            Definition = inspector.Definition;
        }
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
        GetParameterInspector(id).ParseValue(value);
    }

    public RuleInspector GetPostInstruction(string id)
    {
        if (GetAt(PostInstruction + id) is RuleInspector inspector)
            return inspector;
        return null;
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

    private void UpdateOutputPreviews()
    {
        // Update output previews.
        for (int i = 0; i < GetContentsCount(); i++)
        {
            switch (GetAt(i))
            {
                case ParameterInspector parameter:
                    if (parameter is OutputInspector output)
                        output.UpdatePreview(this);
                    break;
            }
        }

        GD.Print(Preview);
    }
}
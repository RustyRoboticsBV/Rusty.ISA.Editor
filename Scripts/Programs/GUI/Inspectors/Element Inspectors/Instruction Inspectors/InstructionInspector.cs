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

    /* Constructors. */
    public InstructionInspector() : base() { }

    public InstructionInspector(InstructionSet set, string opcode) : base()
    {
        Definition = set[opcode];

        // Add pre-instructions.
        foreach (CompileRule rule in Definition.PreInstructions)
        {
            Inspector element = RuleInspectorFactory.Create(set, rule);
            if (element != null)
                Add(PreInstruction + rule.ID, element);
        }

        // Add parameters.
        foreach (Parameter parameter in Definition.Parameters)
        {
            IGuiElement element = ParameterFieldFactory.Create(parameter);
            if (element != null)
                Add(Parameter + parameter.ID, element);
        }

        // Add post-instructions.
        foreach (CompileRule rule in Definition.PostInstructions)
        {
            Inspector element = RuleInspectorFactory.Create(set, rule);
            if (element != null)
                Add(PostInstruction + rule.ID, element);
        }
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

    public IField GetParameterField(string id)
    {
        if (GetAt(Parameter + id) is IField field)
            return field;
        return null;
    }

    public void SetParameterField(string id, string value)
    {
        IField field = GetParameterField(id);
        try
        {
            switch (field)
            {
                case BoolField boolField:
                    boolField.Value = bool.Parse(value);
                    break;
                case IntField intField:
                    intField.Value = int.Parse(value);
                    break;
                case FloatField floatField:
                    floatField.Value = float.Parse(value);
                    break;
                case IntSliderField intSlider:
                    intSlider.Value = int.Parse(value);
                    break;
                case FloatSliderField floatSlider:
                    floatSlider.Value = float.Parse(value);
                    break;
                case CharField charField:
                    charField.Value = char.Parse(value);
                    break;
                case LineField lineField:
                    lineField.Value = value;
                    break;
                case MultilineField multilineField:
                    multilineField.Value = value;
                    break;
                case ColorField colorField:
                    colorField.Value = Color.FromHtml(value);
                    break;
                case OutputField outputField:
                    outputField.Value = value;
                    break;
            }
        }
        catch { }
    }

    public RuleInspector GetPostInstruction(string id)
    {
        if (GetAt(PostInstruction + id) is RuleInspector inspector)
            return inspector;
        return null;
    }
}
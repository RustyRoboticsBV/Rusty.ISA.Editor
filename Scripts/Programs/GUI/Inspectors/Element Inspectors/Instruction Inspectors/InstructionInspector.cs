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
        if (GetAt(PreInstruction + id) is Inspector inspector)
            return inspector as RuleInspector;
        return null;
    }

    public IField GetParameterField(string id)
    {
        if (GetAt(Parameter + id) is IField field)
            return field;
        return null;
    }

    public RuleInspector GetPostInstruction(string id)
    {
        if (GetAt(PostInstruction + id) is Inspector inspector)
            return inspector as RuleInspector;
        return null;
    }
}
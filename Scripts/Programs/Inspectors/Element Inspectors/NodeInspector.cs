namespace Rusty.ISA.Editor;

public partial class NodeInspector : ElementInspector
{
    /* Constants. */
    private const string StartPoint = "start_point";
    private const string Instruction = "instruction";

    /* Public properties. */
    public InstructionDefinition Definition => GetInstructionInspector().Definition;

    /* Constructors. */
    public NodeInspector() : base() { }

    public NodeInspector(InstructionSet set, InstructionDefinition definition) : base(set, definition)
    {
        // Add start point checkbox.
        ToggleTextField startPoint = new();
        startPoint.LabelText = "Start Point";
        startPoint.FieldText = "Start";
        startPoint.TooltipText = "Defines whether or not this node is a start point from which this program can be ran.";
        Add(StartPoint, startPoint);

        // Add instruction inspector.
        InstructionInspector instruction = new(set, definition);
        Add(Instruction, instruction);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        NodeInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public ToggleTextField GetStartPointField()
    {
        return GetAt(StartPoint) as ToggleTextField;
    }

    public InstructionInspector GetInstructionInspector()
    {
        return GetAt(Instruction) as InstructionInspector;
    }
}
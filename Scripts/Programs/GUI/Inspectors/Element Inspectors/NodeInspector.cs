namespace Rusty.ISA.Editor;

public partial class NodeInspector : ElementInspector
{
    /* Constants. */
    private const string StartPoint = "start_point";
    private const string Instruction = "instruction";

    /* Public properties. */
    public new EditorNodePreviewInstance Preview
    {
        get => base.Preview as EditorNodePreviewInstance;
        private set => base.Preview = value;
    }

    /* Constructors. */
    public NodeInspector() : base() { }

    public NodeInspector(InstructionSet set, string opcode) : base(set, opcode)
    {
        // Add start point checkbox.
        ToggleTextField startPoint = new();
        startPoint.LabelText = "Start Point";
        startPoint.FieldText = "Start";
        startPoint.TooltipText = "Defines whether or not this node is a start point from which this program can be ran.";
        Add(StartPoint, startPoint);

        // Add instruction inspector.
        InstructionInspector instruction = new(set, opcode);
        Add(Instruction, instruction);

        // Preview.
        Preview = PreviewDict.ForEditorNode(set[opcode])?.CreateInstance();
        Changed += UpdatePreview;
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

    /* Private methods. */
    private void UpdatePreview()
    {
        var instructionInspector = GetInstructionInspector();

        // Copy preview input from the instruction inspector.
        foreach (var value in instructionInspector.Preview.Input)
        {
            Preview?.Input.SetValue(value.Key, value.Value);
        }

        // Add main preview.
        Preview?.SetMain(instructionInspector.Preview);
    }
}
namespace Rusty.ISA.Editor;

public partial class NodeInspector : ElementInspector
{
    /* Constants. */
    private const string StartPoint = "start_point";
    private const string Instruction = "instruction";

    /* Public properties. */
    public InstructionDefinition Definition => GetInstructionInspector().Definition;

    public new EditorNodePreviewInstance Preview
    {
        get => base.Preview as EditorNodePreviewInstance;
        private set => base.Preview = value;
    }

    /* Constructors. */
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

        // Enable preview.
        EnablePreview();
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        NodeInspector copy = new(InstructionSet, Definition.Opcode);
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
    protected override void UpdatePreview()
    {
        // Init.
        if (Preview == null)
            Preview = PreviewDict.ForEditorNode(InstructionSet, Definition).CreateInstance();

        // Update.
        if (Preview != null)
        {
            var instructionInspector = GetInstructionInspector();

            // Parameters, rules & display name.
            foreach (var value in instructionInspector.Preview.Input)
            {
                Preview.Input.SetValue(value.Key, value.Value);
            }

            // Main.
            Preview.SetMain(instructionInspector.Preview);
        }

        Godot.GD.Print(Preview);
    }
}
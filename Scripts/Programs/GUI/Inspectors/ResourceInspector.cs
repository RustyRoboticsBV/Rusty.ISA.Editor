namespace Rusty.ISA.Editor;

public abstract partial class ResourceInspector : Inspector
{
    /* Public properties. */
    public InstructionSet InstructionSet { get; private set; }
    public PreviewInstance Preview { get; protected set; }

    /* Constructors. */
    public ResourceInspector(InstructionSet instructionSet) : base()
    {
        InstructionSet = instructionSet;
    }

    /* Public methods. */
    public override void CopyFrom(IGuiElement other)
    {
        if (other is ResourceInspector inspector)
        {
            InstructionSet = inspector.InstructionSet;
            Preview = inspector.Preview.Copy();
        }
    }
}
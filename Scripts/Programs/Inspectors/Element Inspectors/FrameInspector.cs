namespace Rusty.ISA.Editor;

public partial class FrameInspector : ElementInspector
{
    /* Constructors. */
    public FrameInspector() : base() { }

    public FrameInspector(InstructionSet set) : base(set, BuiltIn.FrameOpcode)
    {
        InstructionDefinition definition = set[BuiltIn.FrameOpcode];

        // Title field.
        IGuiElement title = ParameterFieldFactory.Create(definition, BuiltIn.FrameTitle);
        Add(BuiltIn.FrameTitle, title);

        // Color field.
        IGuiElement color = ParameterFieldFactory.Create(definition, BuiltIn.FrameColor);
        Add(BuiltIn.FrameColor, color);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        FrameInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public IField GetTitleField()
    {
        return GetAt(BuiltIn.FrameTitle) as IField;
    }

    public IField GetColorField()
    {
        return GetAt(BuiltIn.FrameColor) as IField;
    }
}
namespace Rusty.ISA.Editor;

public partial class FrameInspector : ElementInspector
{
    /* Constructors. */
    public FrameInspector(InstructionSet set) : base(set, BuiltIn.FrameOpcode)
    {
        InstructionDefinition definition = set[BuiltIn.FrameOpcode];

        // Title field.
        ParameterInspector title = new(set, definition.GetParameter(BuiltIn.FrameTitle));
        Add(BuiltIn.FrameTitle, title);

        // Color field.
        ParameterInspector color = new(set, definition.GetParameter(BuiltIn.FrameColor));
        Add(BuiltIn.FrameColor, color);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        FrameInspector copy = new(InstructionSet);
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
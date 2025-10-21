namespace Rusty.ISA.Editor;

public partial class FrameInspector : ElementInspector
{
    /* Constructors. */
    public FrameInspector(InstructionSet set) : base(set, BuiltIn.FrameOpcode)
    {
        // Title field.
        ParameterInspector title = new(set, BuiltIn.FrameOpcode, BuiltIn.FrameTitle);
        Add(BuiltIn.FrameTitle, title);

        // Color field.
        ParameterInspector color = new(set, BuiltIn.FrameOpcode, BuiltIn.FrameColor);
        Add(BuiltIn.FrameColor, color);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        FrameInspector copy = new(InstructionSet);
        copy.CopyFrom(this);
        return copy;
    }

    public ParameterInspector GetTitleField()
    {
        return GetAt(BuiltIn.FrameTitle) as ParameterInspector;
    }

    public ParameterInspector GetColorField()
    {
        return GetAt(BuiltIn.FrameColor) as ParameterInspector;
    }
}
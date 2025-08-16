using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A frame-inspector pair.
/// </summary>
public sealed class LedgerFrame : LedgerItem
{
    /* Public properties. */
    public new GraphFrame Element => base.Element as GraphFrame;
    public new FrameInspector Inspector => base.Inspector as FrameInspector;

    /* Constructors. */
    public LedgerFrame(InstructionSet set, GraphFrame element)
        : base(set, element, new FrameInspector(set))
    {
        OnInspectorChanged();
    }

    /* Protected methods. */
    protected override void OnInspectorChanged()
    {
        // Update frame title & color.
        Element.Title = (string)Inspector.GetTitleField().Value;
        Element.TintColor = (Color)Inspector.GetColorField().Value;
    }
}
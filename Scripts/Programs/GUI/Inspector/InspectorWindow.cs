namespace Rusty.ISA.Editor;

/// <summary>
/// The inspector panel class.
/// </summary>
public sealed partial class InspectorWindow : DockWindow<Inspector>
{
    /* Constructors. */
    public InspectorWindow() : base()
    {
        TitleText = "Inspector";
    }
}
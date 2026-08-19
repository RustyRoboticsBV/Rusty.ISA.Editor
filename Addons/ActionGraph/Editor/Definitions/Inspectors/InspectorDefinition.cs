namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An inspector definition.
/// </summary>
public abstract partial class InspectorDefinition : EditorDefinition
{
    /* Constructors. */
    public InspectorDefinition(string title, string definition) : base(title, definition) { }
}
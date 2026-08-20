namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An inspector definition.
/// </summary>
public abstract partial class InspectorDefinition : EditorDefinition
{
    /* Constructors. */
    public InspectorDefinition(string id, string title, string definition) : base(id, title, definition) { }
}
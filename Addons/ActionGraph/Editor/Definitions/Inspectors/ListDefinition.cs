namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A list inspector definition.
/// </summary>
public sealed partial class ListDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The type of the list's element inspectors.
    /// </summary>
    public InspectorDefinition Type { get; private set; }
    /// <summary>
    /// The text of the "add element" button.
    /// </summary>
    public string AddButtonText { get; private set; } = "";

    /* Constructors. */
    public ListDefinition(string id, string title, string description, InspectorDefinition type, string addButtonText)
        : base(id, title, description)
    {
        Type = type;
        AddButtonText = addButtonText;
    }
}
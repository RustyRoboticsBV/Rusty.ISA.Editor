namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A choice inspector definition.
/// </summary>
public sealed partial class ChoiceDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The available inspector choices.
    /// </summary>
    public InspectorDefinition[] Choices { get; private set; } = [];
    /// <summary>
    /// The initially-selected choice.
    /// </summary>
    public int Selected { get; private set; }

    /* Constructors. */
    public ChoiceDefinition(string id, string title, string description, InspectorDefinition[] choices, int selected)
        : base(id, title, description)
    {
        Choices = choices;
        Selected = selected;
    }
}
namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A tuple inspector definition.
/// </summary>
public sealed partial class TupleDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The tuple's element inspectors.
    /// </summary>
    public InspectorDefinition[] Elements { get; private set; } = [];

    /* Constructors. */
    public TupleDefinition(string id, string title, string description, InspectorDefinition[] elements)
        : base(id, title, description)
    {
        Elements = elements;
    }
}
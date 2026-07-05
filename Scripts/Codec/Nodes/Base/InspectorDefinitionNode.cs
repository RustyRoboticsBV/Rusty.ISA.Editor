namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// An inspector definition node.
/// </summary>
public abstract class InspectorDefinitionNode : CodecNode
{
    /* Public properties. */
    /// <summary>
    /// The form ID.
    /// </summary>
    public string ID { get; set; } = "";

    /* Constructors. */
    public InspectorDefinitionNode(string ID) => this.ID = ID;
}
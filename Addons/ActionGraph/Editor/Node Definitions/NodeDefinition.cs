using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A node definition.
/// </summary>
[GlobalClass]
public partial class NodeDefinition : Resource
{
    /* Public properties. */
    /// <summary>
    /// The ID of the node.
    /// </summary>
    [Export] public string ID { get; set; } = "";
    /// <summary>
    /// The title of the node.
    /// </summary>
    [Export] public string Title { get; set; } = "";
    /// <summary>
    /// The tooltip of the node.
    /// </summary>
    [Export] public string Description { get; set; } = "";
    /// <summary>
    /// The inspectors.
    /// </summary>
    [Export] public InspectorDefinition[] Inspectors { get; set; }
}
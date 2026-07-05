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
    /// The runtime instruction definition.
    /// </summary>
    [Export] public InspectorDefinition[] Inspectors { get; set; }
}
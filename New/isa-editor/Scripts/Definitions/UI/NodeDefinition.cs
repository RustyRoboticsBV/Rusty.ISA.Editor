using Godot;

namespace Rusty.ISA;

/// <summary>
/// An ISA node definition.
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
using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A tuple inspector definition.
/// </summary>
[GlobalClass]
public sealed partial class TupleInspectorDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The elements.
    /// </summary>
    [Export] public InspectorDefinition[] Elements { get; set; }
}
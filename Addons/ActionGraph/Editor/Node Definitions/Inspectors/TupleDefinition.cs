using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A tuple inspector definition.
/// </summary>
[GlobalClass]
public sealed partial class TupleDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The layout direction of the inspector contents.
    /// </summary>
    [Export] public LayoutDirection LayoutDirection { get; set; }
    /// <summary>
    /// The elements.
    /// </summary>
    [Export] public InspectorDefinition[] Elements { get; set; }
}
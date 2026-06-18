using Godot;

namespace Rusty.ISA;

/// <summary>
/// An ISA tuple inspector definition.
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
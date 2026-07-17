using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// An option inspector definition.
/// </summary>
[GlobalClass]
public sealed partial class OptionDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// Whether or not the inspector is enabled by default.
    /// </summary>
    [Export] public bool EnabledByDefault { get; set; }
    /// <summary>
    /// The optional inspector.
    /// </summary>
    [Export] public InspectorDefinition Optional { get; set; }
}
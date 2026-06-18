using Godot;

namespace Rusty.ISA;

/// <summary>
/// An ISA option inspector definition.
/// </summary>
[GlobalClass]
public sealed partial class OptionInspectorDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The optional inspector.
    /// </summary>
    [Export] public InspectorDefinition Optional { get; set; }
    /// <summary>
    /// Whether or not the inspector is enabled by default.
    /// </summary>
    [Export] public bool Enabled { get; set; }
}
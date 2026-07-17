using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// An instruction inspector definition.
/// </summary>
[GlobalClass]
public abstract partial class InspectorDefinition : Resource
{
    /* Public properties. */
    /// <summary>
    /// The ID of the inspector.
    /// </summary>
    [Export] public string ID { get; set; } = "";
    /// <summary>
    /// The display name of the instruction.
    /// </summary>
    [Export] public string Title { get; set; } = "";
    /// <summary>
    /// The tooltip description of the instruction.
    /// </summary>
    [Export] public string Description { get; set; } = "";
    /// <summary>
    /// Whether or not the inspector should be drawn with a foldout.
    /// </summary>
    [Export] public bool Foldable { get; set; }
    /// <summary>
    /// The margins of the inspector.
    /// </summary>
    [Export] public Margins Margins { get; set; }
}
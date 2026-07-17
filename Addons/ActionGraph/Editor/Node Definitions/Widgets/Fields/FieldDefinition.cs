using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A parameter field definition for an instruction inspector.
/// </summary>
[GlobalClass]
public abstract partial class FieldDefinition : WidgetDefinition
{
    /* Public properties. */
    /// <summary>
    /// The ID of the field.
    /// </summary>
    [Export] public string ID { get; set; } = "";
    /// <summary>
    /// The ID of the parameter.
    /// </summary>
    [Export] public string Type { get; set; } = "";
}
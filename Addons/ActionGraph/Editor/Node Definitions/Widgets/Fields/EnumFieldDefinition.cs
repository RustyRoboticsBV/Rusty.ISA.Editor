using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// An enum parameter field definition for an instruction inspector.
/// </summary>
[GlobalClass]
public sealed partial class EnumFieldDefinition : FieldDefinition
{
    /* Public properties. */
    /// <summary>
    /// The possible values.
    /// </summary>
    [Export] public string[] EnumValues { get; private set; }
    /// <summary>
    /// The default value of the field.
    /// </summary>
    [Export] public int DefaultValue { get; private set; }
}
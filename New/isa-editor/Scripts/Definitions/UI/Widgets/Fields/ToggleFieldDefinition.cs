using Godot;

namespace Rusty.ISA;

/// <summary>
/// A toggle parameter field definition for an ISA instruction inspector.
/// </summary>
[GlobalClass]
public sealed partial class ToggleFieldDefinition : FieldDefinition
{
    /* Public properties. */
    /// <summary>
    /// The default value of the field.
    /// </summary>
    [Export] public bool DefaultValue { get; private set; }
}
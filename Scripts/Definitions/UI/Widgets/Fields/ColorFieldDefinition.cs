using Godot;

namespace Rusty.ISA;

/// <summary>
/// A color parameter field definition for an ISA instruction inspector.
/// </summary>
[GlobalClass]
public sealed partial class ColorFieldDefinition : FieldDefinition
{
    /* Public properties. */
    /// <summary>
    /// The default value of the field.
    /// </summary>
    [Export] public Color DefaultValue { get; private set; } = Colors.White;
}
using Godot;

namespace Rusty.ISA;

/// <summary>
/// A text line parameter field definition for an ISA instruction inspector.
/// </summary>
[GlobalClass]
public sealed partial class TextLineFieldDefinition : FieldDefinition
{
    /* Public properties. */
    /// <summary>
    /// The default value of the field.
    /// </summary>
    [Export] public string DefaultValue { get; private set; } = "";
}
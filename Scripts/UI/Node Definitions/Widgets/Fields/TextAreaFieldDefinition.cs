using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A text line parameter area definition for an instruction inspector.
/// </summary>
[GlobalClass]
public sealed partial class TextAreaFieldDefinition : FieldDefinition
{
    /* Public properties. */
    /// <summary>
    /// The default value of the field.
    /// </summary>
    [Export(PropertyHint.MultilineText)] public string DefaultValue { get; private set; } = "";
}
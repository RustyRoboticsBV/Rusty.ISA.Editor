using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A color field definition.
/// </summary>
public sealed partial class ColorDefinition : ParameterDefinition
{
    /* Public properties. */
    /// <summary>
    /// The default value.
    /// </summary>
    public Color DefaultValue { get; private set; } = Colors.White;

    /* Constructors. */
    public ColorDefinition(string id, string title, string description, string type, Color defaultValue)
        : base(id, title, description, type)
    {
        DefaultValue = defaultValue;
    }
}
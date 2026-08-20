using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A string field definition.
/// </summary>
public sealed partial class StringDefinition : ParameterDefinition
{
    /* Public properties. */
    /// <summary>
    /// The default value.
    /// </summary>
    public string DefaultValue { get; private set; }
    /// <summary>
    /// The number of lines.
    /// </summary>
    public int Lines { get; set; } = 1;

    /* Constructors. */
    public StringDefinition(string id, string title, string description, string type, string defaultValue, int lines)
        : base(id, title, description, type)
    {
        DefaultValue = defaultValue;
        Lines = Math.Max(lines, 1);
    }
}
namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An output parameter definition.
/// </summary>
public sealed partial class OutputDefinition : ParameterDefinition
{
    /* Public properties. */
    /// <summary>
    /// Whether or not this output parameter hides the default node output when present.
    /// </summary>
    public bool HidesDefaultOutput { get; private set; }

    /* Constructors. */
    public OutputDefinition(string id, string title, string description, string type, bool hidesDefaultOutput)
        : base(id, title, description, type)
    {
        HidesDefaultOutput = hidesDefaultOutput;
    }
}
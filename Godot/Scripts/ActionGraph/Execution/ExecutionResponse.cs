namespace Rusty.ActionGraph;

/// <summary>
/// The action that the process must make after finishing execution of an instruction.
/// </summary>
public struct ExecutionResponse
{
    /* Public properties. */
    /// <summary>
    /// Advance the program counter.
    /// </summary>
    public static ExecutionResponse Advance => new(ResponseType.Advance);
    /// <summary>
    /// Don't advance the program counter.
    /// </summary>
    public static ExecutionResponse Block => new(ResponseType.Block);
    /// <summary>
    /// Stop program execution.
    /// </summary>
    public static ExecutionResponse Stop => new(ResponseType.Stop);

    /// <summary>
    /// The type of this execution response.
    /// </summary>
    public ResponseType Type { get; private set; }
    /// <summary>
    /// The goto target (only used if the response type is goto).
    /// </summary>
    public string GotoTarget { get; private set; }

    /* Constructors. */
    public ExecutionResponse(ResponseType responseType)
    {
        Type = responseType;
    }

    public ExecutionResponse(string gotoTarget) : this(ResponseType.Goto)
    {
        GotoTarget = gotoTarget;
    }

    /* Public methods. */
    /// <summary>
    /// Jump to an instruction at the label.
    /// </summary>
    public static ExecutionResponse Goto(string label) => new(label);
}
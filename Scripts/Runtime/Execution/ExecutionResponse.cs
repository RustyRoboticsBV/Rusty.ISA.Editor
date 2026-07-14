namespace Rusty.ActionGraph.Runtime;

/// <summary>
/// The action that the process must make after finishing execution of an instruction.
/// </summary>
public struct ExecutionResponse
{
    /* Public types. */
    public enum ResponseType { Advance, Block, Stop, Goto }

    /* Public properties. */
    public static ExecutionResponse Advance => new(ResponseType.Advance);
    public static ExecutionResponse Block => new(ResponseType.Block);
    public static ExecutionResponse Stop => new(ResponseType.Stop);

    public ResponseType Type { get; set; }
    public string GotoTarget { get; set; }

    /* Constructors. */
    public ExecutionResponse(ResponseType action)
    {
        Type = action;
    }

    public ExecutionResponse(string gotoTarget) : this(ResponseType.Goto)
    {
        GotoTarget = gotoTarget;
    }

    /* Public methods. */
    public static ExecutionResponse Goto(string label) => new(label);
}
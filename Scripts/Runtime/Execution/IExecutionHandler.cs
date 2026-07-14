namespace Rusty.ActionGraph.Runtime;

/// <summary>
/// An intercace for instruction execution handlers.
/// </summary>
public interface IExecutionHandler
{
    /* Public properties. */
    /// <summary>
    /// Create an instance of this execution handler.
    /// </summary>
    public abstract static IExecutionHandler Instantiate();

    /// <summary>
    /// Execute an instruction.
    /// </summary>
    public abstract static void Execute(InstructionProcess process, double deltaTime, string[] args);
}

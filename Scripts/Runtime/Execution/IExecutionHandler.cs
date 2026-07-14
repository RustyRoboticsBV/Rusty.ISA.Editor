using System;

namespace Rusty.ActionGraph.Runtime;

/// <summary>
/// An intercace for instruction execution handlers.
/// </summary>
public interface IExecutionHandler
{
    /* Public methods. */
    /// <summary>
    /// Create an instance of this execution handler.
    /// </summary>
    public static IExecutionHandler Instantiate() => throw new NotImplementedException();

    /// <summary>
    /// Execute an instruction. Returns true if the process must advance to the next instruction, and false if it should not.
    /// </summary>
    public ExecutionResponse Execute(InstructionProcess process, double deltaTime, string[] args) => ExecutionResponse.Advance;
}
using Godot;
using System.Collections.Generic;

namespace Rusty.ActionGraph;

/// <summary>
/// An instruction definition. It contains an opcode, parameter IDs and an execution handler name.
/// </summary>
[GlobalClass]
public partial class InstructionDefinition : Resource
{
    /* Public properties. */
    /// <summary>
    /// The unique opcode of the instruction.
    /// </summary>
    [Export] public string Opcode { get; private set; } = "";
    /// <summary>
    /// The parameter IDs of the instruction. Each must be unique within this instruction.
    /// </summary>
    [Export] public string[] Parameters { get; private set; } = [];
    /// <summary>
    /// The name of the execution handler.
    /// </summary>
    [Export] public string ExecutionHandler { get; private set; } = "";

    /* Private properties. */
    private Dictionary<string, int> ParameterIndexCache { get; set; }

    /* Constructors. */
    public InstructionDefinition() : this("", [], "") { }

    public InstructionDefinition(string opcode, string[] parameters, string executionHandler)
    {
        Opcode = opcode;
        Parameters = parameters;
        ExecutionHandler = executionHandler;
    }

    /* Public methods. */
    /// <summary>
    /// Find the index of a parameter. Returns -1 if the parameter does not exist.
    /// </summary>
    public int FindParameter(string id)
    {
        if (ParameterIndexCache == null)
        {
            ParameterIndexCache = new();
            for (int i = 0; i < Parameters.Length; i++)
            {
                ParameterIndexCache.Add(Parameters[i], i);
            }
        }
        
        if (ParameterIndexCache.TryGetValue(id, out int index))
            return index;
        return -1;
    }
}
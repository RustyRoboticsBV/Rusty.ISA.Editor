using Godot;
using System;
using System.Reflection;

namespace Rusty.ActionGraph.Runtime;

/// <summary>
/// A process that can execute an InstructionProgram.
/// </summary>
[GlobalClass]
public partial class InstructionProcess : Node
{
    /* Public properties. */
    [Export] public InstructionProgram Program { get; set; }

    /* Private methods. */
    private static IExecutionHandler FindExecutionHandler(string name)
    {
        // Get type.
        Type type = Type.GetType(name);
        if (type == null)
            throw new TypeAccessException($"Type '{name}' could not be found.");

        // Ensure it's an execution handler.
        if (!type.IsAssignableTo(typeof(IExecutionHandler)))
            throw new InvalidOperationException($"Type '{name}' does not implement {nameof(IExecutionHandler)}.");

        // Find instantiate method.
        MethodInfo method = type.GetMethod(nameof(IExecutionHandler.Instantiate), BindingFlags.Public | BindingFlags.Static, Type.EmptyTypes);
        if (method == null)
            throw new MethodAccessException($"Type '{name}' does not contain a public static parameterless method named '{nameof(IExecutionHandler.Instantiate)}'.");

        // Ensure it returns an execution handler.
        if (!method.ReturnType.IsAssignableTo(typeof(IExecutionHandler)))
            throw new InvalidCastException($"Method '{nameof(IExecutionHandler.Instantiate)}' of type '{type}' does not return an instance of {nameof(IExecutionHandler)}.");

        return method.Invoke(null, null) as IExecutionHandler;
    }
}
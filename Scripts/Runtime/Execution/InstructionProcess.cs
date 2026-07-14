using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rusty.ActionGraph.Runtime;

/// <summary>
/// A process that can execute an InstructionProgram.
/// </summary>
[GlobalClass]
public partial class InstructionProcess : Node
{
    /* Public properties. */
    /// <summary>
    /// The currently-loaded program.
    /// </summary>
    [Export] public InstructionProgram Program { get; private set; }

    /// <summary>
    /// The index of the current instruction.
    /// </summary>
    public int ProgramCounter { get; private set; }
    /// <summary>
    /// Whether the process is running or not.
    /// </summary>
    public bool IsPlaying { get; private set; }
    /// <summary>
    /// Whether the process is paused or not.
    /// </summary>
    public bool IsPaused { get; private set; }

    /* Private properties. */
    private Dictionary<string, int> StartPoints { get; } = new();
    private Dictionary<string, int> Labels { get; } = new();
    private Dictionary<string, IExecutionHandler> ExecutionHandlers { get; } = new();

    /* Public methods. */
    public void LoadProgram(InstructionProgram program)
    {
        // Stop execution.
        Stop();

        // Store program reference.
        Program = program;

        // Find start points and labels.
        StartPoints.Clear();
        Labels.Clear();
        for (int i = 0; i < program.Instructions.Length; i++)
        {
            Instruction instruction = program.Instructions[i];
            if (!string.IsNullOrEmpty(instruction.Start))
                StartPoints.Add(instruction.Start, i);
            if (!string.IsNullOrEmpty(instruction.Label))
                Labels.Add(instruction.Label, i);
        }

        // Find execution handlers.
        ExecutionHandlers.Clear();
        foreach (InstructionDefinition definition in program.InstructionSet.Definitions)
        {
            ExecutionHandlers.Add(definition.Opcode, FindExecutionHandler(definition.ExecutionHandler));
        }
    }

    /// <summary>
    /// Start executing the program from some start point.
    /// </summary>
    public void Play(string startPoint)
    {
        if (Program == null)
            throw new NullReferenceException("No program was loaded.");

        // Move program counter to start point.
        if (StartPoints.TryGetValue(startPoint, out var index))
            ProgramCounter = index;
        else
            throw new KeyNotFoundException($"The start point '{startPoint}' does not exist.");

        // Start playing.
        IsPlaying = true;
    }

    /// <summary>
    /// Stop execution.
    /// </summary>
    public void Stop()
    {
        IsPlaying = false;
        IsPaused = false;
    }

    /// <summary>
    /// Pause execution.
    /// </summary>
    public void Pause()
    {
        if (IsPlaying)
            IsPaused = true;
    }

    /// <summary>
    /// Unpause execution.
    /// </summary>
    public void Unpause()
    {
        IsPaused = false;
    }

    /* Godot overrides. */
    public override void _EnterTree()
    {
        if (Program != null)
            LoadProgram(Program);
    }

    public override void _Process(double delta)
    {
        if (!IsPlaying || IsPaused)
            return;

        // Fetch instruction.
        Instruction instruction = Program.Instructions[ProgramCounter];

        // Execute instruction.
        switch (instruction)
        {
            case GotoInstruction @goto:
                Goto(@goto.TargetLabel);
                break;
            case EndInstruction:
                Stop();
                break;
            case DummyInstruction:
                Advance();
                break;
            case GenericInstruction generic:
                IExecutionHandler handler = ExecutionHandlers[generic.Opcode];
                ExecutionResponse response = handler.Execute(this, delta, generic.Arguments);
                switch (response.Type)
                {
                    case ExecutionResponse.ResponseType.Advance:
                        Advance();
                        break;
                    case ExecutionResponse.ResponseType.Block:
                        break;
                    case ExecutionResponse.ResponseType.Goto:
                        Goto(response.GotoTarget);
                        break;
                    case ExecutionResponse.ResponseType.Stop:
                        Stop();
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown execution response type '{response.Type}'.");
                }
                break;
            default:
                throw new InvalidOperationException($"Unknown instruction type '{instruction.GetType().Name}'.");
        }
    }

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

    /// <summary>
    /// Increment the program counter.
    /// </summary>
    private void Advance()
    {
        ProgramCounter++;
        if (ProgramCounter >= Program.Instructions.Length)
            throw new IndexOutOfRangeException($"{nameof(ProgramCounter)} went out of bounds. All branches of a program must be terminated by end instructions.");
    }

    /// <summary>
    /// Move the program counter to an instruction with some label.
    /// </summary>
    private void Goto(string label)
    {
        if (Labels.TryGetValue(label, out var value))
            ProgramCounter = value;
        else
            throw new KeyNotFoundException($"The label '{label}' does not exist.");
    }
}
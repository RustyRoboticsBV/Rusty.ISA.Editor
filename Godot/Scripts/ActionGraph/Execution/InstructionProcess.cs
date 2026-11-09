using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rusty.ActionGraph;

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
    /// The current localization language. Use "" for unlocalized values.
    /// </summary>
    [Export] public string LocalizationLanguage { get; set; } = "";

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

    private List<string> ArgumentBuffer { get; } = new();

    /* Public methods. */
    /// <summary>
    /// Load a program.
    /// </summary>
    public void LoadProgram(InstructionProgram program)
    {
        // Stop execution.
        Stop();

        // Store program reference.
        Program = program;

        // Find start points and labels.
        StartPoints.Clear();
        Labels.Clear();
        for (int i = 0; i < program.Instructions.Count; i++)
        {
            Instruction instruction = program.Instructions[i];
            if (!string.IsNullOrEmpty(instruction.Start))
                StartPoints.Add(instruction.Start, i);
            if (!string.IsNullOrEmpty(instruction.Label))
                Labels.Add(instruction.Label, i);
        }

        // Find execution handlers.
        ExecutionHandlers.Clear();
        for (int i = 0; i < program.InstructionSet.Count; i++)
        {
            InstructionDefinition definition = program.InstructionSet[i];
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

        ExecutionResponse response = ExecutionResponse.Advance;
        while (response.Type != ResponseType.Block && response.Type != ResponseType.Stop)
        {
            response = Tick(delta);
        }
    }

    /* Private methods. */
    /// <summary>
    /// Find an execution handler.
    /// </summary>
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
    /// Execute a fetch-decode-execute cycle.
    /// </summary>
    private ExecutionResponse Tick(double delta)
    {
        // Fetch instruction.
        Instruction instruction = Program.Instructions[ProgramCounter];

        // Execute instruction.
        ExecutionResponse response;
        switch (instruction)
        {
            case GotoInstruction gto:
                response = ExecutionResponse.Goto(gto.TargetLabel);
                break;
            case EndInstruction:
                response = ExecutionResponse.Stop;
                break;
            case DummyInstruction:
                response = ExecutionResponse.Advance;
                break;
            case GenericInstruction generic:
                // Find the execution handler.
                IExecutionHandler handler = ExecutionHandlers[generic.Opcode];

                // Collect the arguments.
                ArgumentBuffer.Clear();
                for (int i = 0; i < generic.Arguments.Length; i++)
                {
                    string arg = generic.Arguments[i].GetLocalizedValue(LocalizationLanguage);
                    ArgumentBuffer.Add(arg);
                }

                // Execute.
                response = handler.Execute(this, delta, ArgumentBuffer);
                break;
            default:
                throw new InvalidOperationException($"Unknown instruction type '{instruction.GetType().Name}'.");
        }

        // Handle response.
        switch (response.Type)
        {
            case ResponseType.Advance:
                Advance();
                break;
            case ResponseType.Block:
                break;
            case ResponseType.Goto:
                Goto(response.GotoTarget);
                break;
            case ResponseType.Stop:
                Stop();
                break;
            default:
                throw new InvalidOperationException($"Unknown execution response type '{response.Type}'.");
        }

        return response;
    }

    /// <summary>
    /// Increment the program counter.
    /// </summary>
    private void Advance()
    {
        ProgramCounter++;
        if (ProgramCounter >= Program.Instructions.Count)
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
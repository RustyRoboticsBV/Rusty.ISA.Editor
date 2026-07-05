using Godot;
using Godot.Collections;

namespace Rusty.ISA.Runtime;

/// <summary>
/// An ISA program.
/// </summary>
[GlobalClass]
public partial class VirtualProgram : Resource
{
    /* Public properties. */
    [Export] public Dictionary<string, string> Metadata { get; private set; } = new();
    [Export] public Instruction[] Instructions { get; private set; } = [];

    /* Constructors. */
    public VirtualProgram() : this(new(), []) { }

    public VirtualProgram(Dictionary<string, string> metadata, Instruction[] instructions)
    {
        Metadata = metadata;
        Instructions = instructions;
    }
}
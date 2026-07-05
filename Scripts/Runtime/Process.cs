using Godot;

using Rusty.ISA.Runtime;

namespace Rusty.ISA;

/// <summary>
/// A process that can execute an ISA program.
/// </summary>
[GlobalClass]
public partial class Process : Node
{
    /* Public properties. */
    [Export] public VirtualProgram Program { get; set; }
}

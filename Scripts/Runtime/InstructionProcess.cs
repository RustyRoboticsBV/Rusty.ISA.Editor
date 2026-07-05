using Godot;

using Rusty.ActionGraph.Runtime;

namespace Rusty.ActionGraph;

/// <summary>
/// A process that can execute an InstructionProgram.
/// </summary>
[GlobalClass]
public partial class InstructionProcess : Node
{
    /* Public properties. */
    [Export] public InstructionProgram Program { get; set; }
}

using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// An instruction set, consisting of an array of instruction definitions.
/// </summary>
[GlobalClass]
public sealed partial class InstructionSet : Resource
{
    /* Public properties. */
    [Export] public InstructionDefinition[] Definitions { get; private set; }

    /* Constructors. */
    public InstructionSet() : this([]) { }

    public InstructionSet(InstructionDefinition[] definitions) => Definitions = definitions;
}

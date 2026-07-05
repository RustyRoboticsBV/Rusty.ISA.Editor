using Godot;

namespace Rusty.ISA;

/// <summary>
/// An ISA instruction set, consisting of instruction definitions.
/// </summary>
[GlobalClass]
public partial class InstructionSet : Resource
{
    /* Public properties. */
    [Export] public InstructionDefinition[] Definitions { get; private set; }

    /* Constructors. */
    public InstructionSet() : this([]) { }

    public InstructionSet(InstructionDefinition[] definitions) => Definitions = definitions;
}

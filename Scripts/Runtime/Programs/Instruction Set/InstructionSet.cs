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

    /* Public methods. */
    /// <summary>
    /// Find an instruction definition, using its opcode.
    /// </summary>
    public InstructionDefinition Find(string opcode)
    {
        foreach (InstructionDefinition definition in Definitions)
        {
            if (definition.Opcode == opcode)
                return definition;
        }
        return null;
    }
}

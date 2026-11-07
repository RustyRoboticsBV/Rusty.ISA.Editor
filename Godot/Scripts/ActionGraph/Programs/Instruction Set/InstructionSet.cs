using Godot;
using Godot.Collections;
using System.Collections.Generic;

namespace Rusty.ActionGraph;

/// <summary>
/// An instruction set, consisting of an array of instruction definitions.
/// </summary>
[GlobalClass]
public sealed partial class InstructionSet : Resource
{
    /* Fields. */
    [Export] Array<InstructionDefinition> definitions = [];

    /* Public properties. */
    /// <summary>
    /// The number of instruction definitions in the instruction set.
    /// </summary>
    public int Count => definitions.Count;
    
    /* Public indexers. */
    public InstructionDefinition this[int index] => definitions[index];

    /* Constructors. */
    public InstructionSet() { }

    public InstructionSet(InstructionDefinition[] definitions) => this.definitions = new(definitions);

    public InstructionSet(List<InstructionDefinition> definitions)
    {
        foreach (InstructionDefinition definition in definitions)
        {
            this.definitions.Add(definition);
        }
    }

    /* Public methods. */
    /// <summary>
    /// Find an instruction definition, using its opcode.
    /// </summary>
    public InstructionDefinition Find(string opcode)
    {
        foreach (InstructionDefinition definition in definitions)
        {
            if (definition.Opcode == opcode)
                return definition;
        }
        return null;
    }
}

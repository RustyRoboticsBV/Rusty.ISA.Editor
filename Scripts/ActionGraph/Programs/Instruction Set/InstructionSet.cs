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
    /* Private properties. */
    [Export] Array<InstructionDefinition> Definitions { get; set; } = [];

    /* Public properties. */
    public int Count => Definitions.Count;
    
    /* Public indexers. */
    public InstructionDefinition this[int index] => Definitions[index];

    /* Constructors. */
    public InstructionSet() { }

    public InstructionSet(InstructionDefinition[] definitions) => Definitions = new(definitions);

    public InstructionSet(List<InstructionDefinition> definitions)
    {
        foreach (InstructionDefinition definition in definitions)
        {
            Definitions.Add(definition);
        }
    }

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

using Godot;
using Godot.Collections;
using System.Collections.Generic;

namespace Rusty.ActionGraph;

/// <summary>
/// A list of instruction instances.
/// </summary>
[GlobalClass]
public partial class InstructionList : Resource
{
    /* Private properties. */
    [Export] Array<Instruction> instructions = [];

    /* Public properties. */
    /// <summary>
    /// The number of instructions in the list.
    /// </summary>
    public int Count => instructions.Count;

    /* Constructors. */
    public InstructionList() { }

    public InstructionList(Instruction[] instructions) => this.instructions = new(instructions);

    public InstructionList(List<Instruction> instructions)
    {
        foreach (Instruction instruction in instructions)
        {
            this.instructions.Add(instruction);
        }
    }

    /* Public indexers. */
    public Instruction this[int index] => instructions[index];

    /* Public methods. */
    public override string ToString()
    {
        string str = "";
        foreach (Instruction instruction in instructions)
        {
            if (str.Length > 0)
                str += "\n";
            str += instruction.ToString();
        }
        return str;
    }
}
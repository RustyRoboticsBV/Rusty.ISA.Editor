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
    /* Public properties. */
    [Export] Array<Instruction> Instructions { get; set; } = [];
    public int Count => Instructions.Count;

    /* Constructors. */
    public InstructionList() { }

    public InstructionList(Instruction[] instructions) => Instructions = new(instructions);

    public InstructionList(List<Instruction> instructions)
    {
        foreach (Instruction instruction in instructions)
        {
            Instructions.Add(instruction);
        }
    }

    /* Public indexers. */
    public Instruction this[int index] => Instructions[index];

    /* Public methods. */
    public override string ToString()
    {
        string str = "";
        foreach (Instruction instruction in Instructions)
        {
            if (str.Length > 0)
                str += "\n";
            str += instruction.ToString();
        }
        return str;
    }
}
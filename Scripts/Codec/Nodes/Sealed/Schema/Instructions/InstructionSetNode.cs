using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An instruction set node.
/// </summary>
public sealed class InstructionSetNode : ElementNode
{
    /* Constants. */
    public const string TAG = "instrs";

    /* Public properties. */
    /// <summary>
    /// The instruction definition child nodes.
    /// </summary>
    public List<InstructionDefinitionNode> Instructions { get; set; } = new();

    /* Constructors. */
    public InstructionSetNode(List<InstructionDefinitionNode> instructions)
    {
        Instructions = instructions ?? new();
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var instruction in Instructions)
        {
            sb.AppendLine(instruction.Serialize());
        }

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        foreach (var instruction in Instructions)
        {
            instruction?.Hash(hash);
        }
        EndHash(hash, TAG);
    }
}
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An instruction definition node.
/// </summary>
public sealed class InstructionDefinitionNode : ElementNode
{
    /* Constants. */
    public const string TAG = "idef";

    /* Public properties. */
    /// <summary>
    /// The instruction opcode.
    /// </summary>
    public string Opcode { get; set; }
    /// <summary>
    /// The parameter child nodes.
    /// </summary>
    public List<ParameterDefinitionNode> Parameters { get; set; }

    /* Constructors. */
    public InstructionDefinitionNode(string opcode, List<ParameterDefinitionNode> parameters)
    {
        Opcode = opcode;
        Parameters = parameters ?? new();
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var parameter in Parameters)
        {
            sb.AppendLine(parameter.Serialize());
        }

        return Wrap(sb.ToString(), TAG, Opcode);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, Opcode);
        foreach (var parameter in Parameters)
        {
            parameter.Hash(hash);
        }
        EndHash(hash, TAG);
    }
}
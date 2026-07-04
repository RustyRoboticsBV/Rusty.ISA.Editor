using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A schema node.
/// </summary>
public sealed class SchemaNode : ElementNode
{
    /* Constants. */
    public const string TAG = "schema";

    /* Public properties. */
    /// <summary>
    /// The instruction set child node.
    /// </summary>
    public InstructionSetNode Instructions { get; set; }
    /// <summary>
    /// The node set child node.
    /// </summary>
    public NodeSetNode Nodes { get; set; }

    /* Constructors. */
    public SchemaNode(InstructionSetNode instructions, NodeSetNode nodes)
    {
        Instructions = instructions;
        Nodes = nodes;
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        if (Instructions != null)
            sb.AppendLine(Instructions.Serialize());
        if (Nodes != null)
            sb.AppendLine(Nodes.Serialize());
        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Instructions?.Hash(hash);
        Nodes?.Hash(hash);
        EndHash(hash, TAG);
    }
}
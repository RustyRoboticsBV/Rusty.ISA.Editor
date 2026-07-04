using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A metadata node.
/// </summary>
public sealed class MetaNode : CodecNode
{
    /* Constants. */
    public const string TAG = "meta";

    /* Public properties. */
    /// <summary>
    /// The field child nodes.
    /// </summary>
    public List<FieldNode> Fields { get; set; } = new();
    /// <summary>
    /// The checksum child node.
    /// </summary>
    public ChecksumNode Checksum { get; set; }

    /* Constructors. */
    public MetaNode(List<FieldNode> fields, ChecksumNode checksum)
    {
        Fields = fields;
        Checksum = checksum;
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        foreach (FieldNode field in Fields)
        {
            sb.AppendLine(field.Serialize());
        }
        if (Checksum != null)
            sb.AppendLine(Checksum.Serialize());

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        foreach (FieldNode field in Fields)
        {
            field.Hash(hash);
        }
        if (Checksum != null)
            Checksum.Hash(hash);
        EndHash(hash, TAG);
    }
}
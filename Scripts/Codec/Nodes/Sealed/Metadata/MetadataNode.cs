using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A metadata node.
/// </summary>
public sealed class MetadataNode : CodecNode
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
    public MetadataNode(List<FieldNode> fields, ChecksumNode checksum)
    {
        Fields = fields ?? new();
        Checksum = checksum;
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        foreach (FieldNode field in Fields)
        {
            AppendLine(sb, field.Serialize());
        }
        if (Checksum != null)
            AppendLine(sb, Checksum.Serialize());

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

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static MetadataNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        List<FieldNode> fields = new();
        ChecksumNode checksum = null;

        foreach (XmlElement child in xml)
        {
            if (child is XmlNode node)
            {
                switch (child.Name)
                {
                    case FieldNode.TAG:
                        fields.Add(FieldNode.Load(node));
                        break;

                    case ChecksumNode.TAG:
                        checksum = ChecksumNode.Load(node);
                        break;
                }
            }
        }

        return new MetadataNode(fields, checksum);
    }
}
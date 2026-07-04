using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A checksum node.
/// </summary>
public sealed class ChecksumNode : CodecNode
{
    /* Constants. */
    public const string TAG = "check";

    /* Public properties. */
    /// <summary>
    /// The checksum string.
    /// </summary>
    public string String { get; set; } = "";

    /* Constructors. */
    public ChecksumNode(string checksum) => String = checksum;

    /* Public methods. */
    public override string Serialize() => Wrap(String, TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static ChecksumNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        return new(xml.InnerText);
    }
}
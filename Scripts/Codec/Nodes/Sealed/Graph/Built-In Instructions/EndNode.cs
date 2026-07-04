using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An end point node.
/// </summary>
public sealed class EndNode : ElementNode
{
    /* Constants. */
    public const string TAG = "end";

    /* Public methods. */
    public override string Serialize() => Wrap(null, TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static EndNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        return new();
    }
}
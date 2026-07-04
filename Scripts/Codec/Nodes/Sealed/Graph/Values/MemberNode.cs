using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A frame member node.
/// </summary>
public sealed class MemberNode : CodecNode
{
    /* Constants. */
    public const string TAG = "member";

    /* Public properties. */
    /// <summary>
    /// The frame ID.
    /// </summary>
    public string ID { get; set; } = "";

    /* Constructors. */
    public MemberNode(string id) => ID = id;

    /* Public methods. */
    public override string Serialize() => Wrap(null, TAG, ID);

    public override void Hash(HashAlgorithm hash) => EmptyHash(hash, TAG, ID);

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static MemberNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        return new(GetId(xml));
    }
}
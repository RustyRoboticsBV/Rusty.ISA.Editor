using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A start point node.
/// </summary>
public sealed class StartNode : CodecNode
{
    /* Constants. */
    public const string TAG = "start";

    /* Public properties. */
    /// <summary>
    /// The start point name.
    /// </summary>
    public string ID { get; set; } = "";

    /* Constructors. */
    public StartNode(string name) => ID = name;

    /* Public methods. */
    public override string Serialize() => Wrap(null, TAG, ID);

    public override void Hash(HashAlgorithm hash) => EmptyHash(hash, TAG, ID);

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static StartNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        return new(GetId(xml));
    }
}
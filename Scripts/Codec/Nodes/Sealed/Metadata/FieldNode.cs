using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A metadata field node.
/// </summary>
public sealed class FieldNode : CodecNode
{
    /* Constants. */
    public const string TAG = "field";

    /* Public properties. */
    /// <summary>
    /// The metadata ID.
    /// </summary>
    public string ID { get; set; } = "";
    /// <summary>
    /// The metadata value string.
    /// </summary>
    public string Value { get; set; } = "";

    /* Constructors. */
    public FieldNode(string id, string value)
    {
        ID = id;
        Value = value;
    }

    /* Public methods. */
    public override string Serialize() => Wrap(Value, TAG, ID);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        Hash(hash, Value);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static FieldNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        return new FieldNode(GetId(xml), xml.InnerText);
    }
}
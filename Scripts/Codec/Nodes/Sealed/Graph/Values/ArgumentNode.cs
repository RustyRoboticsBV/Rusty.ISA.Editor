using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A form argument node.
/// </summary>
public sealed class ArgumentNode : CodecNode
{
    /* Constants. */
    public const string TAG = "arg";

    /* Public properties. */
    /// <summary>
    /// The argument value.
    /// </summary>
    public string Value { get; set; } = "";

    /* Constructors. */
    public ArgumentNode(string value) => Value = value;

    /* Public methods. */
    public override string Serialize() => Wrap(Value, TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, Value);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static ArgumentNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        return new(xml.InnerText);
    }
}
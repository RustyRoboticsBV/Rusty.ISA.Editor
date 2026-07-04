using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A label node.
/// </summary>
public sealed class LabelNode : ElementNode
{
    /* Constants. */
    public const string TAG = "label";

    /* Public properties. */
    /// <summary>
    /// The label ID.
    /// </summary>
    public string Label { get; set; } = "";

    /* Constructors. */
    public LabelNode(string label) => Label = label;

    /* Public methods. */
    public override string Serialize() => Wrap(null, TAG, Label);

    public override void Hash(HashAlgorithm hash) => EmptyHash(hash, TAG, Label);

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static LabelNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        return new(GetId(xml));
    }
}
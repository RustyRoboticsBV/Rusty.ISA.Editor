using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

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
    public string ID { get; set; } = "";

    /* Constructors. */
    public LabelNode(string label) => ID = label;

    /* Public methods. */
    public override string Serialize() => Wrap(null, TAG, ID);

    public override void Hash(HashAlgorithm hash) => EmptyHash(hash, TAG, ID);

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static LabelNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        return new(GetId(xml));
    }
}
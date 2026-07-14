using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A go-to node.
/// </summary>
public sealed class GotoNode : ElementNode
{
    /* Constants. */
    public const string TAG = "goto";

    /* Public properties. */
    /// <summary>
    /// The goto target label ID.
    /// </summary>
    public string TargetLabel { get; set; } = "";

    /* Constructors. */
    public GotoNode(string targetLabel) => TargetLabel = targetLabel;

    /* Public methods. */
    public override string Serialize() => Wrap(null, TAG, TargetLabel);

    public override void Hash(HashAlgorithm hash) => EmptyHash(hash, TAG, TargetLabel);

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static GotoNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        return new(GetId(xml));
    }

    /// <summary>
    /// Convert to an instruction.
    /// </summary>
    public GotoInstruction ToInstruction() => new(TargetLabel);
}
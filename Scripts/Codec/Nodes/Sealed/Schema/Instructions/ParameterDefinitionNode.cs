using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A parameter definition node.
/// </summary>
public sealed class ParameterDefinitionNode : CodecNode
{
    /* Constants. */
    public const string TAG = "pdef";

    /* Public properties. */
    /// <summary>
    /// The parameter ID.
    /// </summary>
    public string ID { get; set; }

    /* Constructors. */
    public ParameterDefinitionNode(string ID) => this.ID = ID;

    /* Public methods. */
    public override string Serialize() => Wrap(null, TAG, ID);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static ParameterDefinitionNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        return new(GetId(xml));
    }
}
using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A form definition node.
/// </summary>
public sealed class FormDefinitionNode : InspectorDefinitionNode
{
    /* Constants. */
    public const string TAG = "fdef";

    /* Public properties. */
    /// <summary>
    /// The opcode of the form's targeted instruction.
    /// </summary>
    public string Opcode { get; set; }

    /* Constructors. */
    public FormDefinitionNode(string ID, string opcode) : base(ID) => Opcode = opcode;

    /* Public methods. */
    public override string Serialize() => Wrap(Opcode, TAG, ID);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        Hash(hash, Opcode);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static FormDefinitionNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        return new(GetId(xml), xml.InnerText);
    }
}
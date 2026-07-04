using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A list definition node.
/// </summary>
public sealed class ListDefinitionNode : InspectorDefinitionNode
{
    /* Constants. */
    public const string TAG = "ldef";

    /* Public properties. */
    /// <summary>
    /// The element type child node.
    /// </summary>
    public InspectorDefinitionNode Type { get; set; }

    /* Constructors. */
    public ListDefinitionNode(string ID, InspectorDefinitionNode type) : base(ID) => Type = type;

    /* Public methods. */
    public override string Serialize() => Wrap(Type.Serialize(), TAG, ID);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        Type.Serialize();
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static ListDefinitionNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        InspectorDefinitionNode element = null;
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case FormDefinitionNode.TAG:
                    element = FormDefinitionNode.Load(node);
                    break;
                case OptionDefinitionNode.TAG:
                    element = OptionDefinitionNode.Load(node);
                    break;
                case ChoiceDefinitionNode.TAG:
                    element = ChoiceDefinitionNode.Load(node);
                    break;
                case TupleDefinitionNode.TAG:
                    element = TupleDefinitionNode.Load(node);
                    break;
                case TAG:
                    element = Load(node);
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(GetId(xml), element);
    }
}
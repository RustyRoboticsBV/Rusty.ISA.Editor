using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A option definition node.
/// </summary>
public sealed class OptionDefinitionNode : InspectorDefinitionNode
{
    /* Constants. */
    public const string TAG = "odef";

    /* Public properties. */
    /// <summary>
    /// The contained optional child node.
    /// </summary>
    public InspectorDefinitionNode Optional { get; set; }

    /* Constructors. */
    public OptionDefinitionNode(string ID, InspectorDefinitionNode optional) : base(ID) => Optional = optional;

    /* Public methods. */
    public override string Serialize() => Wrap(Optional.Serialize(), TAG, ID);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        Optional.Serialize();
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static OptionDefinitionNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        InspectorDefinitionNode optional = null;
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case FormDefinitionNode.TAG:
                    optional = FormDefinitionNode.Load(node);
                    break;
                case TAG:
                    optional = Load(node);
                    break;
                case ChoiceDefinitionNode.TAG:
                    optional = ChoiceDefinitionNode.Load(node);
                    break;
                case TupleDefinitionNode.TAG:
                    optional = TupleDefinitionNode.Load(node);
                    break;
                case ListDefinitionNode.TAG:
                    optional = ListDefinitionNode.Load(node);
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(GetId(xml), optional);
    }
}
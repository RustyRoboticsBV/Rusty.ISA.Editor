using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A tuple definition node.
/// </summary>
public sealed class TupleDefinitionNode : InspectorDefinitionNode
{
    /* Constants. */
    public const string TAG = "tdef";

    /* Public properties. */
    /// <summary>
    /// The contained tupleal child node.
    /// </summary>
    public List<InspectorDefinitionNode> Elements { get; set; }

    /* Constructors. */
    public TupleDefinitionNode(string ID, List<InspectorDefinitionNode> elements) : base(ID) => Elements = elements;

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        foreach (var element in Elements)
        {
            AppendLine(sb, element.Serialize());
        }

        return Wrap(sb.ToString(), TAG, ID);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        foreach (var tuple in Elements)
        {
            tuple.Hash(hash);
        }
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static TupleDefinitionNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        List<InspectorDefinitionNode> elements = new();
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case FormDefinitionNode.TAG:
                    elements.Add(FormDefinitionNode.Load(node));
                    break;
                case OptionDefinitionNode.TAG:
                    elements.Add(OptionDefinitionNode.Load(node));
                    break;
                case ChoiceDefinitionNode.TAG:
                    elements.Add(ChoiceDefinitionNode.Load(node));
                    break;
                case TAG:
                    elements.Add(Load(node));
                    break;
                case ListDefinitionNode.TAG:
                    elements.Add(ListDefinitionNode.Load(node));
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(GetId(xml), elements);
    }
}
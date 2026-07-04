using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A tuple node.
/// </summary>
public sealed class TupleNode : InspectorNode
{
    /* Constants. */
    public const string TAG = "tuple";

    /* Public properties. */
    /// <summary>
    /// The element child nodes.
    /// </summary>
    public List<InspectorNode> Elements { get; set; } = new();

    /* Constructors. */
    public TupleNode(List<InspectorNode> elements)
    {
        Elements = elements ?? new();
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var inspector in Elements)
        {
            AppendLine(sb, inspector.Serialize());
        }

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        foreach (var element in Elements)
        {
            element?.Hash(hash);
        }
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static TupleNode Load(XmlNode xml)
    {
        List<InspectorNode> elements = new();
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case FormNode.TAG:
                    elements.Add(FormNode.Load(node));
                    break;
                case OptionNode.TAG:
                    elements.Add(OptionNode.Load(node));
                    break;
                case ChoiceNode.TAG:
                    elements.Add(ChoiceNode.Load(node));
                    break;
                case TAG:
                    elements.Add(Load(node));
                    break;
                case ListNode.TAG:
                    elements.Add(ListNode.Load(node));
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(elements);
    }
}
using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An option node.
/// </summary>
public sealed class OptionNode : InspectorNode
{
    /* Constants. */
    public const string TAG = "option";

    /* Public properties. */
    /// <summary>
    /// The child inspector node.
    /// </summary>
    public InspectorNode Enabled { get; set; }

    /* Constructors. */
    public OptionNode(InspectorNode inspector)
    {
        Enabled = inspector;
    }

    /* Public methods. */
    public override string Serialize()
    {
        return Wrap(Enabled?.Serialize(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Enabled?.Hash(hash);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static OptionNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        InspectorNode optional = null;
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case FormNode.TAG:
                    optional = FormNode.Load(node);
                    break;
                case TAG:
                    optional = Load(node);
                    break;
                case ChoiceNode.TAG:
                    optional = ChoiceNode.Load(node);
                    break;
                case TupleNode.TAG:
                    optional = TupleNode.Load(node);
                    break;
                case ListNode.TAG:
                    optional = ListNode.Load(node);
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(optional);
    }
}
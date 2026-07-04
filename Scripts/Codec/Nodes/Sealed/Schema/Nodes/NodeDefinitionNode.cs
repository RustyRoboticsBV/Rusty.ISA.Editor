using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An node definition node.
/// </summary>
public sealed class NodeDefinitionNode : ElementNode
{
    /* Constants. */
    public const string TAG = "ndef";

    /* Public properties. */
    /// <summary>
    /// The node ID.
    /// </summary>
    public string ID { get; set; } = "";
    /// <summary>
    /// The inspector definition child nodes.
    /// </summary>
    public List<InspectorDefinitionNode> Inspectors { get; set; } = new();

    /* Constructors. */
    public NodeDefinitionNode(string ID, List<InspectorDefinitionNode> inspectors)
    {
        this.ID = ID;
        Inspectors = inspectors ?? new();
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var inspector in Inspectors)
        {
            AppendLine(sb, inspector.Serialize());
        }

        return Wrap(sb.ToString(), TAG, ID);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        foreach (var inspector in Inspectors)
        {
            inspector?.Hash(hash);
        }
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static NodeDefinitionNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        List<InspectorDefinitionNode> inspectors = new();
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case FormDefinitionNode.TAG:
                    inspectors.Add(FormDefinitionNode.Load(node));
                    break;
                case OptionDefinitionNode.TAG:
                    inspectors.Add(OptionDefinitionNode.Load(node));
                    break;
                case ChoiceDefinitionNode.TAG:
                    inspectors.Add(ChoiceDefinitionNode.Load(node));
                    break;
                case TupleDefinitionNode.TAG:
                    inspectors.Add(TupleDefinitionNode.Load(node));
                    break;
                case ListDefinitionNode.TAG:
                    inspectors.Add(ListDefinitionNode.Load(node));
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(GetId(xml), inspectors);
    }
}
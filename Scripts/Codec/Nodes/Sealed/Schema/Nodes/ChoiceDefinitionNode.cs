using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A choice definition node.
/// </summary>
public sealed class ChoiceDefinitionNode : InspectorDefinitionNode
{
    /* Constants. */
    public const string TAG = "cdef";

    /* Public properties. */
    /// <summary>
    /// The contained choiceal child node.
    /// </summary>
    public List<InspectorDefinitionNode> Choices { get; set; }

    /* Constructors. */
    public ChoiceDefinitionNode(string ID, List<InspectorDefinitionNode> choices) : base(ID) => Choices = choices;

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        foreach (var choice in Choices)
        {
            AppendLine(sb, choice.Serialize());
        }

        return Wrap(sb.ToString(), TAG, ID);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        foreach (var choice in Choices)
        {
            choice.Hash(hash);
        }
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static ChoiceDefinitionNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        List<InspectorDefinitionNode> choices = new();
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case FormDefinitionNode.TAG:
                    choices.Add(FormDefinitionNode.Load(node));
                    break;
                case OptionDefinitionNode.TAG:
                    choices.Add(OptionDefinitionNode.Load(node));
                    break;
                case TAG:
                    choices.Add(Load(node));
                    break;
                case TupleDefinitionNode.TAG:
                    choices.Add(TupleDefinitionNode.Load(node));
                    break;
                case ListDefinitionNode.TAG:
                    choices.Add(ListDefinitionNode.Load(node));
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(GetId(xml), choices);
    }
}
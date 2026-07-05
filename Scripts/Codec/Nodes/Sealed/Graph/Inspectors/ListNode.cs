using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A list node.
/// </summary>
public sealed class ListNode : InspectorNode
{
    /* Constants. */
    public const string TAG = "list";

    /* Public properties. */
    /// <summary>
    /// The element child nodes.
    /// </summary>
    public List<InspectorNode> Elements { get; set; } = new();

    /* Constructors. */
    public ListNode(List<InspectorNode> elements)
    {
        Elements = elements ?? new();
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
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
    public static ListNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

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
                case TupleNode.TAG:
                    elements.Add(TupleNode.Load(node));
                    break;
                case TAG:
                    elements.Add(Load(node));
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(elements);
    }

    /// <summary>
    /// Convert to a list of instructions.
    /// </summary>
    public List<Instruction> ToInstructions(SchemaNode schema, ListDefinitionNode definition)
    {
        List<Instruction> instructions = new();
        foreach (var element in Elements)
        {
            List<Instruction> sub = null;
            switch (element)
            {
                case FormNode form:
                    sub = form.ToInstructions(schema, definition.Type as FormDefinitionNode);
                    break;
                case OptionNode option:
                    sub = option.ToInstructions(schema, definition.Type as OptionDefinitionNode);
                    break;
                case ChoiceNode choice:
                    sub = choice.ToInstructions(schema, definition.Type as ChoiceDefinitionNode);
                    break;
                case TupleNode tuple:
                    sub = tuple.ToInstructions(schema, definition.Type as TupleDefinitionNode);
                    break;
                case ListNode list:
                    sub = list.ToInstructions(schema, definition.Type as ListDefinitionNode);
                    break;
                default:
                    throw BadNodeException(this, element);
            }
            AppendLists(instructions, sub);
        }
        return instructions;
    }
}
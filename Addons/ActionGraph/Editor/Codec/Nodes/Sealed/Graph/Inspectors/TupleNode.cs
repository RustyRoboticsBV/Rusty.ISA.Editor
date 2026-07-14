using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

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

    /// <summary>
    /// Convert to a list of instructions.
    /// </summary>
    public List<Instruction> ToInstructions(SchemaNode schema, TupleDefinitionNode definition)
    {
        List<Instruction> instructions = new();
        for (int i = 0; i < Elements.Count; i++)
        {
            List<Instruction> sub = null;
            switch (Elements[i])
            {
                case FormNode form:
                    sub = form.ToInstructions(schema, definition.Elements[i] as FormDefinitionNode);
                    break;
                case OptionNode option:
                    sub = option.ToInstructions(schema, definition.Elements[i] as OptionDefinitionNode);
                    break;
                case ChoiceNode choice:
                    sub = choice.ToInstructions(schema, definition.Elements[i] as ChoiceDefinitionNode);
                    break;
                case TupleNode tuple:
                    sub = tuple.ToInstructions(schema, definition.Elements[i] as TupleDefinitionNode);
                    break;
                case ListNode list:
                    sub = list.ToInstructions(schema, definition.Elements[i] as ListDefinitionNode);
                    break;
                default:
                    throw BadNodeException(this, Elements[i]);
            }
            AppendLists(instructions, sub);
        }
        return instructions;
    }
}
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// An choice node.
/// </summary>
public sealed class ChoiceNode : InspectorNode
{
    /* Constants. */
    public const string TAG = "choice";

    /* Public properties. */
    /// <summary>
    /// The index child node.
    /// </summary>
    public IndexNode Index { get; set; }
    /// <summary>
    /// The child inspector node.
    /// </summary>
    public InspectorNode Selected { get; set; }

    /* Constructors. */
    public ChoiceNode(IndexNode index, InspectorNode inspector)
    {
        Index = index;
        Selected = inspector;
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        if (Index != null)
            AppendLine(sb, Index.Serialize());
        if (Selected != null)
            AppendLine(sb, Selected.Serialize());
        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Index?.Hash(hash);
        Selected?.Hash(hash);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static ChoiceNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        IndexNode index = null;
        InspectorNode selected = null;
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case IndexNode.TAG:
                    index = IndexNode.Load(node);
                    break;
                case FormNode.TAG:
                    selected = FormNode.Load(node);
                    break;
                case OptionNode.TAG:
                    selected = OptionNode.Load(node);
                    break;
                case TAG:
                    selected = Load(node);
                    break;
                case TupleNode.TAG:
                    selected = TupleNode.Load(node);
                    break;
                case ListNode.TAG:
                    selected = ListNode.Load(node);
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(index, selected);
    }

    /// <summary>
    /// Convert to a list of instructions.
    /// </summary>
    public List<Instruction> ToInstructions(SchemaNode schema, ChoiceDefinitionNode definition)
    {
        switch (Selected)
        {
            case FormNode form:
                return form.ToInstructions(schema, definition.Choices[Index.Index] as FormDefinitionNode);
            case OptionNode option:
                return option.ToInstructions(schema, definition.Choices[Index.Index] as OptionDefinitionNode);
            case ChoiceNode choice:
                return choice.ToInstructions(schema, definition.Choices[Index.Index] as ChoiceDefinitionNode);
            case TupleNode tuple:
                return tuple.ToInstructions(schema, definition.Choices[Index.Index] as TupleDefinitionNode);
            case ListNode list:
                return list.ToInstructions(schema, definition.Choices[Index.Index] as ListDefinitionNode);
            default:
                throw BadNodeException(this, Selected);
        }
    }
}
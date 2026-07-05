using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

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

    /// <summary>
    /// Convert to a list of instructions.
    /// </summary>
    public List<Instruction> ToInstructions(SchemaNode schema, OptionDefinitionNode definition)
    {
        switch (Enabled)
        {
            case null:
                return [];
            case FormNode form:
                return form.ToInstructions(schema, definition.Optional as FormDefinitionNode);
            case OptionNode option:
                return option.ToInstructions(schema, definition.Optional as OptionDefinitionNode);
            case ChoiceNode choice:
                return choice.ToInstructions(schema, definition.Optional as ChoiceDefinitionNode);
            case TupleNode tuple:
                return tuple.ToInstructions(schema, definition.Optional as TupleDefinitionNode);
            case ListNode list:
                return list.ToInstructions(schema, definition.Optional as ListDefinitionNode);
            default:
                throw BadNodeException(this, Enabled);
        }
    }
}
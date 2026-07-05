using Godot;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A graph node.
/// </summary>
public sealed class GraphNode : ElementNode
{
    /* Constants. */
    public const string TAG = "graph";

    /* Public properties. */
    /// <summary>
    /// The element child nodes.
    /// </summary>
    public List<ElementNode> Elements { get; set; } = new();

    /* Constructors. */
    public GraphNode(List<ElementNode> elements)
    {
        Elements = elements ?? new();
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        foreach (var element in Elements)
        {
            AppendLine(sb, element.Serialize());
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
    public static GraphNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        List<ElementNode> elements = new();
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case JointNode.TAG:
                    elements.Add(JointNode.Load(node));
                    break;
                case MemoNode.TAG:
                    elements.Add(MemoNode.Load(node));
                    break;
                case FrameNode.TAG:
                    elements.Add(FrameNode.Load(node));
                    break;
                case NodeNode.TAG:
                    elements.Add(NodeNode.Load(node));
                    break;
                case GblockNode.TAG:
                    elements.Add(GblockNode.Load(node));
                    break;
                case EblockNode.TAG:
                    elements.Add(EblockNode.Load(node));
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
    public List<Instruction> ToInstructions(SchemaNode schema)
    {
        List<Instruction> instructions = new();
        foreach (var element in Elements)
        {
            switch (element)
            {
                case NodeNode node:
                    AppendLists(instructions, node.ToInstructions(schema));
                    break;
                case JointNode joint:
                    instructions.Add(joint.ToInstruction());
                    break;
                case GblockNode gblock:
                    instructions.Add(gblock.ToInstruction());
                    break;
                case EblockNode eblock:
                    instructions.Add(eblock.ToInstruction());
                    break;
                case FrameNode frame:
                case MemoNode memo:
                    break;
                default:
                    throw BadNodeException(this, element);
            }
        }
        return instructions;
    }
}
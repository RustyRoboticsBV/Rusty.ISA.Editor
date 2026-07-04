using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A schema node.
/// </summary>
public sealed class SchemaNode : ElementNode
{
    /* Constants. */
    public const string TAG = "schema";

    /* Public properties. */
    /// <summary>
    /// The instruction set child node.
    /// </summary>
    public InstructionSetNode Instructions { get; set; }
    /// <summary>
    /// The node set child node.
    /// </summary>
    public NodeSetNode Nodes { get; set; }

    /* Constructors. */
    public SchemaNode(InstructionSetNode instructions, NodeSetNode nodes)
    {
        Instructions = instructions;
        Nodes = nodes;
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        if (Instructions != null)
            AppendLine(sb, Instructions.Serialize());
        if (Nodes != null)
            AppendLine(sb, Nodes.Serialize());
        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Instructions?.Hash(hash);
        Nodes?.Hash(hash);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static SchemaNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        InstructionSetNode instructionSet = null;
        NodeSetNode nodeSet = null;
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case InstructionSetNode.TAG:
                    instructionSet = InstructionSetNode.Load(node);
                    break;
                case NodeSetNode.TAG:
                    nodeSet = NodeSetNode.Load(node);
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(instructionSet, nodeSet);
    }
}
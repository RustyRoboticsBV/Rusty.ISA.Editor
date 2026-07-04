using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An node set node.
/// </summary>
public sealed class NodeSetNode : CodecNode
{
    /* Constants. */
    public const string TAG = "nodes";

    /* Public properties. */
    /// <summary>
    /// The node definition child nodes.
    /// </summary>
    public List<NodeDefinitionNode> Nodes { get; set; } = new();

    /* Constructors. */
    public NodeSetNode(List<NodeDefinitionNode> nodes)
    {
        Nodes = nodes ?? new();
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var node in Nodes)
        {
            AppendLine(sb, node.Serialize());
        }

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        foreach (var node in Nodes)
        {
            node?.Hash(hash);
        }
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static NodeSetNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        List<NodeDefinitionNode> nodes = new();
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case NodeDefinitionNode.TAG:
                    nodes.Add(NodeDefinitionNode.Load(node));
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(nodes);
    }
}
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

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
            sb.AppendLine(node.Serialize());
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
}
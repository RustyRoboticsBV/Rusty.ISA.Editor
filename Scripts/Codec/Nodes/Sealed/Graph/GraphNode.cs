using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

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
        StringBuilder sb = new StringBuilder();
        foreach (var element in Elements)
        {
            sb.AppendLine(element.Serialize());
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
}
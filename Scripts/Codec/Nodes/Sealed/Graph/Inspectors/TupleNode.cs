using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A tuple node.
/// </summary>
public sealed class TupleNode : ElementNode
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
        StringBuilder sb = new StringBuilder();
        foreach (var inspector in Elements)
        {
            sb.AppendLine(inspector.Serialize());
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
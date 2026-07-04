using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A node node.
/// </summary>
public sealed class NodeNode : ElementNode
{
    /* Constants. */
    public const string TAG = "node";

    /* Public properties. */
    /// <summary>
    /// The x-position child node.
    /// </summary>
    public XNode X { get; set; }
    /// <summary>
    /// The y-position child node.
    /// </summary>
    public YNode Y { get; set; }
    /// <summary>
    /// The frame member child node.
    /// </summary>
    public MemberNode Member { get; set; }
    /// <summary>
    /// The start point child node.
    /// </summary>
    public StartNode Start { get; set; }
    /// <summary>
    /// The inspector child nodes.
    /// </summary>
    public List<InspectorNode> Inspectors { get; set; } = new();

    /* Constructors. */
    public NodeNode(XNode x, YNode y, MemberNode member, StartNode start, List<InspectorNode> inspectors)
    {
        X = x;
        Y = y;
        Member = member;
        Start = start;
        Inspectors = inspectors ?? new();
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new StringBuilder();

        if (X != null)
            sb.AppendLine(X.Serialize());
        if (Y != null)
            sb.AppendLine(Y.Serialize());
        if (Member != null)
            sb.AppendLine(Member.Serialize());
        if (Start != null)
            sb.AppendLine(Start.Serialize());

        foreach (var inspector in Inspectors)
        {
            sb.AppendLine(inspector.Serialize());
        }

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        X?.Hash(hash);
        Y?.Hash(hash);
        Member?.Hash(hash);
        Start?.Hash(hash);
        foreach (var inspector in Inspectors)
        {
            inspector.Hash(hash);
        }
        EndHash(hash, TAG);
    }
}
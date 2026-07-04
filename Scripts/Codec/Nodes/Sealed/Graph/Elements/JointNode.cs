using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A joint node.
/// </summary>
public sealed class JointNode : ElementNode
{
    /* Constants. */
    public const string TAG = "joint";

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

    /* Constructors. */
    public JointNode(XNode x, YNode y, MemberNode member)
    {
        X = x;
        Y = y;
        Member = member;
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

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        X?.Hash(hash);
        Y?.Hash(hash);
        Member?.Hash(hash);
        EndHash(hash, TAG);
    }
}
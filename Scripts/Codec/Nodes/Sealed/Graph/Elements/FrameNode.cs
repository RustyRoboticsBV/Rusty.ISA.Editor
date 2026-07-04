using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A frame node.
/// </summary>
public sealed class FrameNode : ElementNode
{
    /* Constants. */
    public const string TAG = "frame";

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
    /// The width child node.
    /// </summary>
    public WidthNode Width { get; set; }
    /// <summary>
    /// The height child node.
    /// </summary>
    public HeightNode Height { get; set; }
    /// <summary>
    /// The frame member child node.
    /// </summary>
    public MemberNode Member { get; set; }
    /// <summary>
    /// The frame header text child node.
    /// </summary>
    public TextNode Text { get; set; }
    /// <summary>
    /// The frame color child node.
    /// </summary>
    public ColorNode Color { get; set; }

    /* Constructors. */
    public FrameNode(XNode x, YNode y, WidthNode width, HeightNode height, MemberNode member, TextNode text, ColorNode color)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Member = member;
        Text = text;
        Color = color;
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new StringBuilder();

        if (X != null)
            sb.AppendLine(X.Serialize());
        if (Y != null)
            sb.AppendLine(Y.Serialize());
        if (Width != null)
            sb.AppendLine(Width.Serialize());
        if (Height != null)
            sb.AppendLine(Height.Serialize());
        if (Member != null)
            sb.AppendLine(Member.Serialize());
        if (Text != null)
            sb.AppendLine(Text.Serialize());
        if (Color != null)
            sb.AppendLine(Color.Serialize());

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        X?.Hash(hash);
        Y?.Hash(hash);
        Width?.Hash(hash);
        Height?.Hash(hash);
        Member?.Hash(hash);
        Text?.Hash(hash);
        Color?.Hash(hash);
        EndHash(hash, TAG);
    }
}
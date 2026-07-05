using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A comment node.
/// </summary>
public sealed class MemoNode : ElementNode
{
    /* Constants. */
    public const string TAG = "memo";

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
    /// The frame header text child node.
    /// </summary>
    public TextNode Text { get; set; }
    /// <summary>
    /// The frame color child node.
    /// </summary>
    public ColorNode Color { get; set; }

    /* Constructors. */
    public MemoNode(XNode x, YNode y, MemberNode member, TextNode text, ColorNode color)
    {
        X = x;
        Y = y;
        Member = member;
        Text = text;
        Color = color;
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();

        if (X != null)
            AppendLine(sb, X.Serialize());
        if (Y != null)
            AppendLine(sb, Y.Serialize());
        if (Member != null)
            AppendLine(sb, Member.Serialize());
        if (Text != null)
            AppendLine(sb, Text.Serialize());
        if (Color != null)
            AppendLine(sb, Color.Serialize());

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        X?.Hash(hash);
        Y?.Hash(hash);
        Member?.Hash(hash);
        Text?.Hash(hash);
        Color?.Hash(hash);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static MemoNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        XNode x = null;
        YNode y = null;
        MemberNode member = null;
        TextNode text = null;
        ColorNode color = null;
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case XNode.TAG:
                    x = XNode.Load(node);
                    break;
                case YNode.TAG:
                    y = YNode.Load(node);
                    break;
                case MemberNode.TAG:
                    member = MemberNode.Load(node);
                    break;
                case TextNode.TAG:
                    text = TextNode.Load(node);
                    break;
                case ColorNode.TAG:
                    color = ColorNode.Load(node);
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(x, y, member, text, color);
    }
}
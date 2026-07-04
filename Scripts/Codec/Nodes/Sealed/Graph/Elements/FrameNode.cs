using System.Security.Cryptography;
using System.Text;
using System.Xml;

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
    /// The ID of the frame.
    /// </summary>
    public string ID { get; set; } = "";
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
    public FrameNode(string ID, XNode x, YNode y, WidthNode width, HeightNode height, MemberNode member, TextNode text, ColorNode color)
    {
        this.ID = ID;
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
            AppendLine(sb, X.Serialize());
        if (Y != null)
            AppendLine(sb, Y.Serialize());
        if (Width != null)
            AppendLine(sb, Width.Serialize());
        if (Height != null)
            AppendLine(sb, Height.Serialize());
        if (Member != null)
            AppendLine(sb, Member.Serialize());
        if (Text != null)
            AppendLine(sb, Text.Serialize());
        if (Color != null)
            AppendLine(sb, Color.Serialize());

        return Wrap(sb.ToString(), TAG, ID);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        X?.Hash(hash);
        Y?.Hash(hash);
        Width?.Hash(hash);
        Height?.Hash(hash);
        Member?.Hash(hash);
        Text?.Hash(hash);
        Color?.Hash(hash);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static FrameNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        XNode x = null;
        YNode y = null;
        WidthNode width = null;
        HeightNode height = null;
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
                case WidthNode.TAG:
                    width = WidthNode.Load(node);
                    break;
                case HeightNode.TAG:
                    height = HeightNode.Load(node);
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
        return new(GetId(xml), x, y, width, height, member, text, color);
    }
}
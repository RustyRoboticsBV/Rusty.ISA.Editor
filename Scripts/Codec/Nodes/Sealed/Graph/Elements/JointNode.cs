using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
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
    /// <summary>
    /// The label child node.
    /// </summary>
    public LabelNode Label { get; set; }

    /* Constructors. */
    public JointNode(XNode x, YNode y, MemberNode member, LabelNode label)
    {
        X = x;
        Y = y;
        Member = member;
        Label = label;
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new StringBuilder();

        if (X != null)
            AppendLine(sb, X.Serialize());
        if (Y != null)
            AppendLine(sb, Y.Serialize());
        if (Member != null)
            AppendLine(sb, Member.Serialize());
        if (Label != null)
            AppendLine(sb, Label.Serialize());

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        X?.Hash(hash);
        Y?.Hash(hash);
        Member?.Hash(hash);
        Label?.Hash(hash);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static JointNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        XNode x = null;
        YNode y = null;
        MemberNode member = null;
        StartNode start = null;
        LabelNode label = null;
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
                case StartNode.TAG:
                    start = StartNode.Load(node);
                    break;
                case LabelNode.TAG:
                    label = LabelNode.Load(node);
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(x, y, member, label);
    }

    /// <summary>
    /// Convert to a list of instructions.
    /// </summary>
    public Instruction ToInstruction()
    {
        DummyInstruction instruction = new();
        if (Label != null)
            instruction.Label = Label.ID;
        return instruction;
    }
}
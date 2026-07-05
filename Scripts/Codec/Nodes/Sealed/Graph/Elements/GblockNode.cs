using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A go-to block node.
/// </summary>
public sealed class GblockNode : ElementNode
{
    /* Constants. */
    public const string TAG = "gblock";

    /* Public properties. */
    /// <summary>
    /// The label child node.
    /// </summary>
    public LabelNode Label { get; set; }
    /// <summary>
    /// The goto child node.
    /// </summary>
    public GotoNode Goto { get; set; }

    /* Constructors. */
    public GblockNode(LabelNode label, GotoNode @goto)
    {
        Label = label;
        Goto = @goto;
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();

        if (Label != null)
            AppendLine(sb, Label.Serialize());
        if (Goto != null)
            AppendLine(sb, Goto.Serialize());

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Label?.Hash(hash);
        Goto?.Hash(hash);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static GblockNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        LabelNode label = null;
        GotoNode @goto = null;
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case LabelNode.TAG:
                    label = LabelNode.Load(node);
                    break;
                case GotoNode.TAG:
                    @goto = GotoNode.Load(node);
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(label, @goto);
    }

    /// <summary>
    /// Convert to an instruction.
    /// </summary>
    public GotoInstruction ToInstruction()
    {
        GotoInstruction instruction = Goto?.ToInstruction();
        if (instruction != null && Label != null)
            instruction.Label = Label.ID;
        return instruction;
    }
}
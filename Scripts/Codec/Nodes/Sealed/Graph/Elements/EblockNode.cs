using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// An end block node.
/// </summary>
public sealed class EblockNode : ElementNode
{
    /* Constants. */
    public const string TAG = "eblock";

    /* Public properties. */
    /// <summary>
    /// The label child node.
    /// </summary>
    public LabelNode Label { get; set; }
    /// <summary>
    /// The end child node.
    /// </summary>
    public EndNode End { get; set; }

    /* Constructors. */
    public EblockNode(LabelNode label, EndNode end)
    {
        Label = label;
        End = end;
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();

        if (Label != null)
            AppendLine(sb, Label.Serialize());
        if (End != null)
            AppendLine(sb, End.Serialize());

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Label?.Hash(hash);
        End?.Hash(hash);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static EblockNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        LabelNode label = null;
        EndNode end = null;
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case LabelNode.TAG:
                    label = LabelNode.Load(node);
                    break;
                case EndNode.TAG:
                    end = EndNode.Load(node);
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(label, end);
    }

    /// <summary>
    /// Convert to an instruction.
    /// </summary>
    public EndInstruction ToInstruction()
    {
        // Generate instruction instance.
        EndInstruction instruction = End?.ToInstruction();
        if (instruction != null && Label != null)
            instruction.Label = Label.ID;

        // Generate resource name.
        StringBuilder resourceName = new();
        if (instruction != null && Label != null)
        {
            resourceName.Append(Label.ID);
            resourceName.Append(": ");
        }
        resourceName.Append("END");
        instruction.ResourceName = resourceName.ToString();

        return instruction;
    }
}
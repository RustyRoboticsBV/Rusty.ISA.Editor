using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An instruction set node.
/// </summary>
public sealed class InstructionSetNode : ElementNode
{
    /* Constants. */
    public const string TAG = "instrs";

    /* Public properties. */
    /// <summary>
    /// The instruction definition child nodes.
    /// </summary>
    public List<InstructionDefinitionNode> Instructions { get; set; } = new();

    /* Constructors. */
    public InstructionSetNode(List<InstructionDefinitionNode> instructions)
    {
        Instructions = instructions ?? new();
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        foreach (var instruction in Instructions)
        {
            AppendLine(sb, instruction.Serialize());
        }

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        foreach (var instruction in Instructions)
        {
            instruction?.Hash(hash);
        }
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static InstructionSetNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        List<InstructionDefinitionNode> instructions = new();
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case InstructionDefinitionNode.TAG:
                    instructions.Add(InstructionDefinitionNode.Load(node));
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(instructions);
    }
}
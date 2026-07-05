using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A form node.
/// </summary>
public sealed class FormNode : InspectorNode
{
    /* Constants. */
    public const string TAG = "form";

    /* Public properties. */
    /// <summary>
    /// The form arguments.
    /// </summary>
    public List<ArgumentNode> Arguments { get; set; } = new();

    /* Constructors. */
    public FormNode(List<ArgumentNode> arguments) => Arguments = arguments;

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var arg in Arguments)
        {
            AppendLine(sb, arg.Serialize());
        }

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        foreach (var arg in Arguments)
        {
            arg?.Hash(hash);
        }
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static FormNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        List<ArgumentNode> arguments = new();
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case ArgumentNode.TAG:
                    arguments.Add(ArgumentNode.Load(node));
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(arguments);
    }

    /// <summary>
    /// Convert to a list of instructions.
    /// </summary>
    public List<Instruction> ToInstructions(SchemaNode schema, FormDefinitionNode definition)
    {
        GenericInstruction instruction = new(definition.Opcode, new string[Arguments.Count]);
        for (int i = 0; i < Arguments.Count; i++)
        {
            instruction.Arguments[i] = Arguments[i].Value;
        }
        return [instruction];
    }
}
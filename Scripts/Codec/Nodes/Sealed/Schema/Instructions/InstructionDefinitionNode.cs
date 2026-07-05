using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An instruction definition node.
/// </summary>
public sealed class InstructionDefinitionNode : ElementNode
{
    /* Constants. */
    public const string TAG = "idef";

    /* Public properties. */
    /// <summary>
    /// The instruction opcode.
    /// </summary>
    public string Opcode { get; set; }
    /// <summary>
    /// The execution handler child node.
    /// </summary>
    public ExecutionHandlerNode ExecutionHandler { get; set; }
    /// <summary>
    /// The parameter child nodes.
    /// </summary>
    public List<ParameterDefinitionNode> Parameters { get; set; }

    /* Constructors. */
    public InstructionDefinitionNode(string opcode, ExecutionHandlerNode executionHandler, List<ParameterDefinitionNode> parameters)
    {
        Opcode = opcode;
        ExecutionHandler = executionHandler;
        Parameters = parameters ?? new();
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        if (ExecutionHandler != null)
            AppendLine(sb, ExecutionHandler.Serialize());
        foreach (var parameter in Parameters)
        {
            AppendLine(sb, parameter.Serialize());
        }

        return Wrap(sb.ToString(), TAG, Opcode);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, Opcode);
        if (ExecutionHandler != null)
            ExecutionHandler.Hash(hash);
        foreach (var parameter in Parameters)
        {
            parameter.Hash(hash);
        }
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static InstructionDefinitionNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        ExecutionHandlerNode executionHandler = null;
        List<ParameterDefinitionNode> parameters = new();
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case ExecutionHandlerNode.TAG:
                    executionHandler = ExecutionHandlerNode.Load(node);
                    break;
                case ParameterDefinitionNode.TAG:
                    parameters.Add(ParameterDefinitionNode.Load(node));
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(GetId(xml), executionHandler, parameters);
    }
}
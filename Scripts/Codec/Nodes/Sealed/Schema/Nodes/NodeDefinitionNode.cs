using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An node definition node.
/// </summary>
public sealed class NodeDefinitionNode : ElementNode
{
    /* Constants. */
    public const string TAG = "ndef";

    /* Public properties. */
    /// <summary>
    /// The node ID.
    /// </summary>
    public string ID { get; set; } = "";
    /// <summary>
    /// The inspector definition child nodes.
    /// </summary>
    public List<InspectorDefinitionNode> Inspectors { get; set; } = new();

    /* Constructors. */
    public NodeDefinitionNode(string ID, List<InspectorDefinitionNode> inspectors)
    {
        this.ID = ID;
        Inspectors = inspectors ?? new();
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var inspector in Inspectors)
        {
            sb.AppendLine(inspector.Serialize());
        }

        return Wrap(sb.ToString(), TAG, ID);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        foreach (var inspector in Inspectors)
        {
            inspector?.Hash(hash);
        }
        EndHash(hash, TAG);
    }
}
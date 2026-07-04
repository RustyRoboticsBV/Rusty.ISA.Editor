using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A tuple definition node.
/// </summary>
public sealed class TupleDefinitionNode : InspectorDefinitionNode
{
    /* Constants. */
    public const string TAG = "tdef";

    /* Public properties. */
    /// <summary>
    /// The contained tupleal child node.
    /// </summary>
    public List<InspectorDefinitionNode> Elements { get; set; }

    /* Constructors. */
    public TupleDefinitionNode(string ID, List<InspectorDefinitionNode> elements) : base(ID) => Elements = elements;

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        foreach (var element in Elements)
        {
            sb.AppendLine(element.Serialize());
        }

        return Wrap(sb.ToString(), TAG, ID);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        foreach (var tuple in Elements)
        {
            tuple.Hash(hash);
        }
        EndHash(hash, TAG);
    }
}
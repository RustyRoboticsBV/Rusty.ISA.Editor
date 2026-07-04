using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A choice definition node.
/// </summary>
public sealed class ChoiceDefinitionNode : InspectorDefinitionNode
{
    /* Constants. */
    public const string TAG = "cdef";

    /* Public properties. */
    /// <summary>
    /// The contained choiceal child node.
    /// </summary>
    public List<InspectorDefinitionNode> Choices { get; set; }

    /* Constructors. */
    public ChoiceDefinitionNode(string ID, List<InspectorDefinitionNode> choices) : base(ID) => Choices = choices;

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        foreach (var choice in Choices)
        {
            sb.AppendLine(choice.Serialize());
        }

        return Wrap(sb.ToString(), TAG, ID);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        foreach (var choice in Choices)
        {
            choice.Hash(hash);
        }
        EndHash(hash, TAG);
    }
}
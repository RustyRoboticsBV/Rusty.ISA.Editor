using System.Security.Cryptography;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A list definition node.
/// </summary>
public sealed class ListDefinitionNode : InspectorDefinitionNode
{
    /* Constants. */
    public const string TAG = "ldef";

    /* Public properties. */
    /// <summary>
    /// The element type child node.
    /// </summary>
    public InspectorDefinitionNode Type { get; set; }

    /* Constructors. */
    public ListDefinitionNode(string ID, InspectorDefinitionNode type) : base(ID) => Type = type;

    /* Public methods. */
    public override string Serialize() => Wrap(Type.Serialize(), TAG, ID);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        Type.Serialize();
        EndHash(hash, TAG);
    }
}
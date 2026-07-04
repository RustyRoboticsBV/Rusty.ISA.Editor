using System.Security.Cryptography;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A parameter definition node.
/// </summary>
public sealed class ParameterDefinitionNode : CodecNode
{
    /* Constants. */
    public const string TAG = "idef";

    /* Public properties. */
    /// <summary>
    /// The parameter ID.
    /// </summary>
    public string ID { get; set; }

    /* Constructors. */
    public ParameterDefinitionNode(string ID) => this.ID = ID;

    /* Public methods. */
    public override string Serialize() => Wrap(null, TAG, ID);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        EndHash(hash, TAG);
    }
}
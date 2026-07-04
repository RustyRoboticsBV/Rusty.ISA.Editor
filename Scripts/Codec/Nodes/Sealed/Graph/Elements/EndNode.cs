using System.Security.Cryptography;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An end point node.
/// </summary>
public sealed class EndNode : ElementNode
{
    /* Constants. */
    public const string TAG = "end";

    /* Public methods. */
    public override string Serialize() => Wrap(null, TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        EndHash(hash, TAG);
    }
}
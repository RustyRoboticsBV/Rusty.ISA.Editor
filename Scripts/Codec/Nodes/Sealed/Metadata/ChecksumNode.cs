using System.Security.Cryptography;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A checksum node.
/// </summary>
public sealed class ChecksumNode : CodecNode
{
    /* Constants. */
    public const string TAG = "check";

    /* Public properties. */
    /// <summary>
    /// The checksum string.
    /// </summary>
    public string Checksum { get; set; } = "";

    /* Constructors. */
    public ChecksumNode(string checksum) => Checksum = checksum;

    /* Public methods. */
    public override string Serialize() => Wrap(Checksum, TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        EndHash(hash, TAG);
    }
}
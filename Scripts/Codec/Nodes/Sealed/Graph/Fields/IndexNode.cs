using System.Globalization;
using System.Security.Cryptography;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An index node.
/// </summary>
public sealed class IndexNode : CodecNode
{
    /* Constants. */
    public const string TAG = "idx";

    /* Public properties. */
    /// <summary>
    /// The index.
    /// </summary>
    public int Index { get; set; }

    /* Constructors. */
    public IndexNode(int index) => Index = index;

    /* Public methods. */
    public override string Serialize() => Wrap(Index.ToString(), TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, Index.ToString(CultureInfo.InvariantCulture));
        EndHash(hash, TAG);
    }
}
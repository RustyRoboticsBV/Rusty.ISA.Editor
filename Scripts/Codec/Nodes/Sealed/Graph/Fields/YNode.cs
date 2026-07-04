using System.Globalization;
using System.Security.Cryptography;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An element y-position node.
/// </summary>
public sealed class YNode : CodecNode
{
    /* Constants. */
    public const string TAG = "y";

    /* Public properties. */
    /// <summary>
    /// The y position.
    /// </summary>
    public double Y { get; set; }

    /* Constructors. */
    public YNode(double value) => Y = value;

    /* Public methods. */
    public override string Serialize() => Wrap(Y.ToString(), TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, Y.ToString(CultureInfo.InvariantCulture));
        EndHash(hash, TAG);
    }
}
using System.Globalization;
using System.Security.Cryptography;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An element width node.
/// </summary>
public sealed class WidthNode : CodecNode
{
    /* Constants. */
    public const string TAG = "width";

    /* Public properties. */
    /// <summary>
    /// The element width.
    /// </summary>
    public double Width { get; set; }

    /* Constructors. */
    public WidthNode(double value) => Width = value;

    /* Public methods. */
    public override string Serialize() => Wrap(Width.ToString(), TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, Width.ToString(CultureInfo.InvariantCulture));
        EndHash(hash, TAG);
    }
}
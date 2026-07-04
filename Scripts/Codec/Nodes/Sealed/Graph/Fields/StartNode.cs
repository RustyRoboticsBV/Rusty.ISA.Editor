using System.Security.Cryptography;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A start point node.
/// </summary>
public sealed class StartNode : CodecNode
{
    /* Constants. */
    public const string TAG = "start";

    /* Public properties. */
    /// <summary>
    /// The start point name.
    /// </summary>
    public string Name { get; set; } = "";

    /* Constructors. */
    public StartNode(string name) => Name = name;

    /* Public methods. */
    public override string Serialize() => Wrap(Name, TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, Name);
        EndHash(hash, TAG);
    }
}
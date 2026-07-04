using System.Security.Cryptography;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A frame member node.
/// </summary>
public sealed class MemberNode : CodecNode
{
    /* Constants. */
    public const string TAG = "member";

    /* Public properties. */
    /// <summary>
    /// The frame ID.
    /// </summary>
    public string ID { get; set; } = "";

    /* Constructors. */
    public MemberNode(string id) => ID = id;

    /* Public methods. */
    public override string Serialize() => Wrap(ID, TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, ID);
        EndHash(hash, TAG);
    }
}
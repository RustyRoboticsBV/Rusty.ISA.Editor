using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A go-to node.
/// </summary>
public sealed class GotoNode : ElementNode
{
    /* Constants. */
    public const string TAG = "goto";

    /* Public properties. */
    /// <summary>
    /// The goto target label ID.
    /// </summary>
    public string TargetLabel { get; set; } = "";

    /* Constructors. */
    public GotoNode(string targetLabel) => TargetLabel = targetLabel;

    /* Public methods. */
    public override string Serialize() => Wrap(TargetLabel, TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, TargetLabel);
        EndHash(hash, TAG);
    }
}
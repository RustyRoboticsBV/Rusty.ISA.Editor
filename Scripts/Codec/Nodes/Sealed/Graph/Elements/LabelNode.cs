using System.Security.Cryptography;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A label node.
/// </summary>
public sealed class LabelNode : ElementNode
{
    /* Constants. */
    public const string TAG = "label";

    /* Public properties. */
    /// <summary>
    /// The label ID.
    /// </summary>
    public string Label { get; set; } = "";

    /* Constructors. */
    public LabelNode(string label) => Label = label;

    /* Public methods. */
    public override string Serialize() => Wrap(Label, TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, Label);
        EndHash(hash, TAG);
    }
}
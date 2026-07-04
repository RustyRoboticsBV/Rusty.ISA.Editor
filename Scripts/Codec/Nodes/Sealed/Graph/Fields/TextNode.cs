using System.Security.Cryptography;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A text node.
/// </summary>
public sealed class TextNode : CodecNode
{
    /* Constants. */
    public const string TAG = "text";

    /* Public properties. */
    /// <summary>
    /// The text string.
    /// </summary>
    public string Text { get; set; } = "";

    /* Constructors. */
    public TextNode(string text) => Text = text;

    /* Public methods. */
    public override string Serialize() => Wrap(Text, TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, Text);
        EndHash(hash, TAG);
    }
}
using Godot;
using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A color node.
/// </summary>
public sealed class ColorNode : CodecNode
{
    /* Constants. */
    public const string TAG = "color";

    /* Public properties. */
    /// <summary>
    /// The text string.
    /// </summary>
    public Color Color { get; set; } = Colors.White;

    /* Constructors. */
    public ColorNode(Color color) => Color = color;

    /* Public methods. */
    public override string Serialize() => Wrap(Color.ToHtml(Color.A < 1f), TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, Color.ToHtml(Color.A < 1f));
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static ColorNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        return new(Color.FromHtml(xml.InnerText));
    }
}
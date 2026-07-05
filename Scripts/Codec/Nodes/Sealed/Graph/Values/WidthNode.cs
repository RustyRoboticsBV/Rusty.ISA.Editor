using System.Globalization;
using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

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

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static WidthNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        if (double.TryParse(xml.InnerText, out double value))
            return new(value);
        else
            return new(0.0);
    }
}
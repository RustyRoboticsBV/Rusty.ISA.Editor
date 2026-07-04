using System.Globalization;
using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An element height node.
/// </summary>
public sealed class HeightNode : CodecNode
{
    /* Constants. */
    public const string TAG = "height";

    /* Public properties. */
    /// <summary>
    /// The element height.
    /// </summary>
    public double Height { get; set; }

    /* Constructors. */
    public HeightNode(double value) => Height = value;

    /* Public methods. */
    public override string Serialize() => Wrap(Height.ToString(), TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, Height.ToString(CultureInfo.InvariantCulture));
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static HeightNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        if (double.TryParse(xml.InnerText, out double value))
            return new(value);
        else
            return new(0.0);
    }
}
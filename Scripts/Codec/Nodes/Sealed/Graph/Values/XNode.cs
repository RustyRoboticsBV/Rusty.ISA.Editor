using System.Globalization;
using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An element x-position node.
/// </summary>
public sealed class XNode : CodecNode
{
    /* Constants. */
    public const string TAG = "x";

    /* Public properties. */
    /// <summary>
    /// The x position.
    /// </summary>
    public double X { get; set; }

    /* Constructors. */
    public XNode(double value) => X = value;

    /* Public methods. */
    public override string Serialize() => Wrap(X.ToString(), TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, X.ToString(CultureInfo.InvariantCulture));
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static XNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        if (double.TryParse(xml.InnerText, out double value))
            return new(value);
        else
            return new(0.0);
    }
}
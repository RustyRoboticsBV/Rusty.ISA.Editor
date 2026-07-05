using System.Globalization;
using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// An element y-position node.
/// </summary>
public sealed class YNode : CodecNode
{
    /* Constants. */
    public const string TAG = "y";

    /* Public properties. */
    /// <summary>
    /// The y position.
    /// </summary>
    public double Y { get; set; }

    /* Constructors. */
    public YNode(double value) => Y = value;

    /* Public methods. */
    public override string Serialize() => Wrap(Y.ToString(), TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, Y.ToString(CultureInfo.InvariantCulture));
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static YNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        if (double.TryParse(xml.InnerText, out double value))
            return new(value);
        else
            return new(0.0);
    }
}
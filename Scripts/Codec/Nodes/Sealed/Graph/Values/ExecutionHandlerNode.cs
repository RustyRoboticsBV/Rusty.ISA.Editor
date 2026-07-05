using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An execution handler node.
/// </summary>
public sealed class ExecutionHandlerNode : CodecNode
{
    /* Constants. */
    public const string TAG = "exec";

    /* Public properties. */
    /// <summary>
    /// The execution handler class name (with namespace).
    /// </summary>
    public string Name { get; set; } = "";

    /* Constructors. */
    public ExecutionHandlerNode(string name) => Name = name;

    /* Public methods. */
    public override string Serialize() => Wrap(Name, TAG);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Hash(hash, Name);
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static ExecutionHandlerNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        return new(xml.InnerText);
    }
}
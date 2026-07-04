using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An option node.
/// </summary>
public sealed class OptionNode : ElementNode
{
    /* Constants. */
    public const string TAG = "option";

    /* Public properties. */
    /// <summary>
    /// The child inspector node.
    /// </summary>
    public InspectorNode Enabled { get; set; }

    /* Constructors. */
    public OptionNode(InspectorNode inspector)
    {
        Enabled = inspector;
    }

    /* Public methods. */
    public override string Serialize()
    {
        return Wrap(Enabled?.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Enabled?.Hash(hash);
        EndHash(hash, TAG);
    }
}
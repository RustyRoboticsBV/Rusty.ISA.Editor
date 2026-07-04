using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// An choice node.
/// </summary>
public sealed class ChoiceNode : ElementNode
{
    /* Constants. */
    public const string TAG = "choice";

    /* Public properties. */
    /// <summary>
    /// The index child node.
    /// </summary>
    public IndexNode Index { get; set; }
    /// <summary>
    /// The child inspector node.
    /// </summary>
    public InspectorNode Selected { get; set; }

    /* Constructors. */
    public ChoiceNode(InspectorNode inspector)
    {
        Selected = inspector;
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        if (Index != null)
            sb.AppendLine(Index.Serialize());
        if (Selected != null)
            sb.AppendLine(Selected.Serialize());
        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Index?.Hash(hash);
        Selected?.Hash(hash);
        EndHash(hash, TAG);
    }
}
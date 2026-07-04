using System.Security.Cryptography;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A option definition node.
/// </summary>
public sealed class OptionDefinitionNode : InspectorDefinitionNode
{
    /* Constants. */
    public const string TAG = "odef";

    /* Public properties. */
    /// <summary>
    /// The contained optional child node.
    /// </summary>
    public InspectorDefinitionNode Choices { get; set; }

    /* Constructors. */
    public OptionDefinitionNode(string ID, InspectorDefinitionNode optional) : base(ID) => Choices = optional;

    /* Public methods. */
    public override string Serialize() => Wrap(Choices.Serialize(), TAG, ID);

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        Choices.Serialize();
        EndHash(hash, TAG);
    }
}
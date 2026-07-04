using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A form node.
/// </summary>
public sealed class FormNode : InspectorNode
{
    /* Constants. */
    public const string TAG = "form";

    /* Public properties. */
    /// <summary>
    /// The form arguments.
    /// </summary>
    public List<ArgumentNode> Arguments { get; set; } = new();

    /* Constructors. */
    public FormNode(List<ArgumentNode> arguments) => Arguments = arguments;

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var arg in Arguments)
        {
            sb.AppendLine(arg.Serialize());
        }

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        foreach (var arg in Arguments)
        {
            arg?.Hash(hash);
        }
        EndHash(hash, TAG);
    }
}
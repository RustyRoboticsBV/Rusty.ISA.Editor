using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.Compilation;

/// <summary>
/// A compiler unit representing a graph joint.
/// </summary>
internal class JointUnit : MonoUnit
{
    /* Public properties. */
    /// <summary>
    /// The loaded joint data from the program file.
    /// </summary>
    public JointCodec Codec { get; private set; }

    /* Constructors */
    public JointUnit(JointCodec codec) => Codec = codec;

    /* Public methods. */
    public override string ToString() => Codec?.ToString(true) ?? "";
}

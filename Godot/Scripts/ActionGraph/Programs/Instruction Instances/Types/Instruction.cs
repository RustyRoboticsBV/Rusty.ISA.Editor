using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// An instruction instance.
/// </summary>
[GlobalClass]
public abstract partial class Instruction : Resource
{
    /* Public properties. */
    /// <summary>
    /// The start name of this instruction. If not empty, this instruction is an entry point.
    /// </summary>
    [Export] public string Start { get; set; } = null;
    /// <summary>
    /// The label name of this instruction. If not empty, this instruction can be jumped to.
    /// </summary>
    [Export] public string Label { get; set; } = null;

    /* Public methods. */
    public override string ToString()
    {
        bool hasStart = !string.IsNullOrEmpty(Start);
        bool hasLabel = !string.IsNullOrEmpty(Label);
        if (hasStart && hasLabel)
            return $"[{Start}] {Label}: ";
        else if (hasStart)
            return $"[{Start}] ";
        else if (hasLabel)
            return $"{Label}: ";
        else
            return "";
    }
}
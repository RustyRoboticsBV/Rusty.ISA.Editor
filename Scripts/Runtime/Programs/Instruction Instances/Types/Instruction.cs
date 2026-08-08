using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// An instruction instance.
/// </summary>
[GlobalClass]
public abstract partial class Instruction : Resource
{
    /* Public properties. */
    [Export] public string Start { get; set; } = null;
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
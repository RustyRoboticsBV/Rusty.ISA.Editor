using Godot;
using Godot.Collections;

namespace Rusty.ActionGraph;

/// <summary>
/// An argument of a generic instruction.
/// </summary>
[GlobalClass]
public sealed partial class InstructionArgument : Resource
{
    /* Public properties. */
    /// <summary>
    /// The unlocalized value of the argument.
    /// </summary>
    [Export] public string Value { get; private set; } = "";
    /// <summary>
    /// The value of the argument when localized to different languages.
    /// </summary>
    [Export] public Dictionary<string, string> Localized { get; private set; } = new();

    /* Constructors. */
    public InstructionArgument() : this("", null) { }

    public InstructionArgument(string value, Dictionary<string, string> localized = null)
    {
        Value = value;
        Localized = localized ?? new();
        ResourceName = ToString();
    }

    /* Public methods. */
    public override string ToString() => Value;

    public string GetLocalizedValue(string language) => Localized.TryGetValue(language, out string value) ? value : Value;
}
using Godot;
using Godot.Collections;

namespace Rusty.ISA.Editor;

/// <summary>
/// The input of a preview calculator.
/// </summary>
[GlobalClass]
public partial class PreviewInput : Resource
{
    /* Public properties. */
    /// <summary>
    /// The accesible preview values.
    /// </summary>
    [Export] public Dictionary<string, Variant> Values { get; private set; } = new();

    /* Public methods. */
    /// <summary>
    /// Add a value to the preview input dictionary.
    /// </summary>
    public void AddValue(string key, Variant value)
    {
        Values.Add(key, value);
    }

    /// <summary>
    /// Get a value from the preview input values. Returns the empty string if the key did not exist.
    /// </summary>
    public Variant GetValue(string key)
    {
        if (Values.ContainsKey(key))
            return Values[key];
        return "ERROR: MISSING VALUE " + key;
    }
}
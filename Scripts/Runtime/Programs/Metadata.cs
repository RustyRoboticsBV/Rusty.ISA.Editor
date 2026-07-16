using Godot;
using Godot.Collections;

namespace Rusty.ActionGraph.Runtime;

/// <summary>
/// A program metadata container.
/// </summary>
public partial class Metadata : Resource
{
    /* Public properties. */
    [Export] public Dictionary<string, string> Values { get; private set; } = new();

    public string Title => GetValue("title");
    public string Description => GetValue("desc");
    public string Author => GetValue("author");
    public string Version => GetValue("version");

    /* Constructors. */
    public Metadata() { }

    public Metadata(Dictionary<string, string> values) => Values = values;

    /* Public methods. */
    /// <summary>
    /// Add a value.
    /// </summary>
    public void AddValue(string key, string value) => Values.Add(key, value);

    /// <summary>
    /// Get a value.
    /// </summary>
    public string GetValue(string key) => Values.TryGetValue("version", out string value) ? value : "";
}
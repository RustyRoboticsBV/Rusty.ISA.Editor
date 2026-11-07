using Godot;
using Godot.Collections;

namespace Rusty.ActionGraph;

/// <summary>
/// A program metadata container.
/// </summary>
[GlobalClass]
public partial class Metadata : Resource
{
    /* Fields. */
    /// <summary>
    /// The metadata keys and values.
    /// </summary>
    [Export] Dictionary<string, string> values = new();

    /* Public properties. */
    public string Title => GetValue("title");
    public string Description => GetValue("desc");
    public string Author => GetValue("author");
    public string Version => GetValue("version");

    /* Constructors. */
    public Metadata() { }

    public Metadata(Dictionary<string, string> values) => this.values = values;

    /* Public methods. */
    /// <summary>
    /// Add a value.
    /// </summary>
    public void AddValue(string key, string value) => values.Add(key, value);

    /// <summary>
    /// Get a value.
    /// </summary>
    public string GetValue(string key) => values.TryGetValue(key, out string value) ? value : "";
}
using Godot;
using Godot.Collections;

namespace Rusty.ISA.Editor;

/// <summary>
/// The input of a preview calculator.
/// </summary>
public sealed partial class PreviewInput : Resource, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, Variant>>
{
    /* Public properties. */
    /// <summary>
    /// The accesible preview values.
    /// </summary>
    [Export] public Dictionary<string, Variant> Values { get; private set; } = new();

    /* Public methods. */
    public override string ToString()
    {
        string str = "{";
        foreach (var value in Values)
        {
            string valueStr = value.Value.ToString();
            if (!(valueStr.StartsWith('{') && valueStr.EndsWith('}')))
                valueStr = $"\"{valueStr}\"";

            str += "\n " + value.Key + " = " + valueStr.Replace("\n", "\n ");
        }
        str += "\n}";
        return str.Replace("{\n}", "{ }");
    }

    /// <summary>
    /// Make a deep copy of this object. This recursively copies nested preview instances.
    /// </summary>
    public PreviewInput Copy()
    {
        PreviewInput copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    /// <summary>
    /// Copy the values from another input object. This recursively copies nested preview instances.
    /// </summary>
    public void CopyFrom(PreviewInput other)
    {
        Values.Clear();
        foreach (var value in other.Values)
        {
            if (value.Value.AsGodotObject() is PreviewInstance preview)
                SetValue(value.Key, preview.Copy());
            else
                SetValue(value.Key, value.Value);
        }
    }

    /// <summary>
    /// Add a value to the preview input dictionary.
    /// </summary>
    public void AddValue(string key)
    {
        Values.Add(key, "");
    }

    /// <summary>
    /// Get a value from the preview input values. Returns the empty string if the key did not exist.
    /// </summary>
    public Variant GetValue(string key)
    {
        if (Values.ContainsKey(key))
        {
            Variant value = Values[key];
            if (value.AsGodotObject() is PreviewInstance preview)
                return preview.Evaluate();
            else
                return value;
        }
        return "ERROR: MISSING VALUE " + key;
    }

    /// <summary>
    /// Set a value on the preview input values. Adds the value if it wasn't added yet.
    /// </summary>
    public void SetValue(string key, Variant value)
    {
        if (!Values.ContainsKey(key))
            AddValue(key);
        Values[key] = value;
    }

    /* Enumerating. */
    public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<string, Variant>> GetEnumerator()
    {
        return Values.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
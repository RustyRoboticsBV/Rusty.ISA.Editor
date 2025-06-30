using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A bi-directional dictionary.
/// </summary>
public class BiDict<T1, T2>
{
    /* Public properties. */
    public int Count => Forward.Count;
    /// <summary>
    /// The keys on the left side of the bidirectional dictionary.
    /// </summary>
    public Dictionary<T1, T2>.KeyCollection LeftValues => Forward.Keys;
    /// <summary>
    /// The keys on the right side of the bidirectional dictionary.
    /// </summary>
    public Dictionary<T2, T1>.KeyCollection RightValues => Inverse.Keys;

    /* Private properties. */
    private Dictionary<T1, T2> Forward { get; } = new();
    private Dictionary<T2, T1> Inverse { get; } = new();

    /* Indexers. */
    public T1 this[T2 value] => Inverse[value];
    public T2 this[T1 value] => Forward[value];

    /* Public methods. */
    /// <summary>
    /// Get the first value of some value pair, using its second value as the key.
    /// </summary>
    public T1 GetLeft(T2 value)
    {
        return Inverse[value];
    }

    /// <summary>
    /// Get the second value of some value pair, using its first value as the key.
    /// </summary>
    public T2 GetRight(T1 value)
    {
        return Forward[value];
    }

    /// <summary>
    /// Try to get the pair of some value.
    /// </summary>
    public bool TryGetLeft(T1 value, out T2 result)
    {
        return Forward.TryGetValue(value, out result);
    }

    /// <summary>
    /// Try to get the pair of some value.
    /// </summary>
    public bool TryGetRight(T2 value, out T1 result)
    {
        return Inverse.TryGetValue(value, out result);
    }

    /// <summary>
    /// Returns if a value pair exists in the bidirectional dictionary, with some first value.
    /// </summary>
    public bool ContainsLeft(T1 value)
    {
        return Forward.ContainsKey(value);
    }

    /// <summary>
    /// Returns if a value pair exists in the bidirectional dictionary, with some second value.
    /// </summary>
    public bool ContainsRight(T2 value)
    {
        return Inverse.ContainsKey(value);
    }

    /// <summary>
    /// Adds the specified value pair to the bidirectional dictionary.
    /// </summary>
    public void Add(T1 value1, T2 value2)
    {
        Forward.Add(value1, value2);
        Inverse.Add(value2, value1);
    }

    /// <summary>
    /// Removes a value and its pair from the bidirectional dictionary.
    /// </summary>
    public void Remove(T1 value)
    {
        T2 other = GetRight(value);
        Forward.Remove(value);
        Inverse.Remove(other);
    }

    /// <summary>
    /// Removes a value and its pair from the bidirectional dictionary.
    /// </summary>
    public void Remove(T2 value)
    {
        T1 other = GetLeft(value);
        Forward.Remove(other);
        Inverse.Remove(value);
    }

    /// <summary>
    /// Remove all value pairs from the bidirectional dictionary.
    /// </summary>
    public void Clear()
    {
        Forward.Clear();
        Inverse.Clear();
    }
}
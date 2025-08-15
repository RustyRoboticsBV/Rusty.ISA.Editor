using System.Collections;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A bi-directional dictionary.
/// </summary>
public class BiDict<T1, T2> : IEnumerable<(T1, T2)>
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
    public T1 this[T2 rightValue] => Inverse[rightValue];
    public T2 this[T1 leftValue] => Forward[leftValue];

    /* Public methods. */
    /// <summary>
    /// Get the first value of some value pair, using its second value as the key.
    /// </summary>
    public T1 GetLeft(T2 rightValue)
    {
        return Inverse[rightValue];
    }

    /// <summary>
    /// Get the second value of some value pair, using its first value as the key.
    /// </summary>
    public T2 GetRight(T1 leftValue)
    {
        return Forward[leftValue];
    }

    /// <summary>
    /// Try to get the pair of some value.
    /// </summary>
    public bool TryGetLeft(T2 rightValue, out T1 leftValue)
    {
        return Inverse.TryGetValue(rightValue, out leftValue);
    }

    /// <summary>
    /// Try to get the pair of some value.
    /// </summary>
    public bool TryGetRight(T1 leftValue, out T2 rightValue)
    {
        return Forward.TryGetValue(leftValue, out rightValue);
    }

    /// <summary>
    /// Returns if a value pair exists in the bidirectional dictionary, with some first value.
    /// </summary>
    public bool ContainsLeft(T1 leftValue)
    {
        return Forward.ContainsKey(leftValue);
    }

    /// <summary>
    /// Returns if a value pair exists in the bidirectional dictionary, with some second value.
    /// </summary>
    public bool ContainsRight(T2 rightValue)
    {
        return Inverse.ContainsKey(rightValue);
    }

    /// <summary>
    /// Adds the specified value pair to the bidirectional dictionary.
    /// </summary>
    public void Add(T1 leftValue, T2 rightValue)
    {
        Forward.Add(leftValue, rightValue);
        Inverse.Add(rightValue, leftValue);
    }

    /// <summary>
    /// Removes a value and its pair from the bidirectional dictionary.
    /// </summary>
    public void Remove(T1 leftValue)
    {
        T2 other = GetRight(leftValue);
        Forward.Remove(leftValue);
        Inverse.Remove(other);
    }

    /// <summary>
    /// Removes a value and its pair from the bidirectional dictionary.
    /// </summary>
    public void Remove(T2 rightValue)
    {
        T1 other = GetLeft(rightValue);
        Forward.Remove(other);
        Inverse.Remove(rightValue);
    }

    /// <summary>
    /// Remove all value pairs from the bidirectional dictionary.
    /// </summary>
    public void Clear()
    {
        Forward.Clear();
        Inverse.Clear();
    }

    /* Enumeration. */
    public IEnumerator<(T1, T2)> GetEnumerator()
    {
        foreach (var pair in Forward)
        {
            yield return (pair.Key, pair.Value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
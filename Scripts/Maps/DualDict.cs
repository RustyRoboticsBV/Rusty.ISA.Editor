using System.Collections;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A dictionary where there are two keys per value. You can use either to access the value.
/// </summary>
public class DualDict<T1, T2, T3> : IEnumerable<T3>
{
    /* Public properties. */
    public int Count => Keys.Count;

    /* Private properties. */
    private Dictionary<T1, T3> A { get; } = new();
    private Dictionary<T2, T3> B { get; } = new();
    private BiDict<T1, T2> Keys { get; set; } = new();

    /* Indexers. */
    public T3 this[T1 key] => A[key];
    public T3 this[T2 key] => B[key];

    /* Public methods. */
    /// <summary>
    /// Adds the specified key-key-value pair to the dictionary.
    /// </summary>
    public void Add(T1 key1, T2 key2, T3 value)
    {
        A.Add(key1, value);
        B.Add(key2, value);
        Keys.Add(key1, key2);
    }

    /// <summary>
    /// Remove a key-key-value pair from the dictionary.
    /// </summary>
    public void Remove(T1 key)
    {
        A.Remove(key);
        B.Remove(Keys[key]);
        Keys.Remove(key);
    }

    /// <summary>
    /// Remove a key-key-value pair from the dictionary.
    /// </summary>
    public void Remove(T2 key)
    {
        B.Remove(key);
        A.Remove(Keys[key]);
        Keys.Remove(key);
    }

    /// <summary>
    /// Remove all keys and values from the dictionary.
    /// </summary>
    public void Clear()
    {
        A.Clear();
        B.Clear();
        Keys.Clear();
    }

    /* IEnumerable. */
    public IEnumerator<T3> GetEnumerator()
    {
        foreach (T3 value in A.Values)
        {
            yield return value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
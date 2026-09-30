using System;

namespace Rusty.ActionGraph.Serialization;

internal static class ArrayMerger
{
    /* Public methods. */
    public static T[] Merge<T>(T[] a, T[] b)
    {
        T[] result = new T[a.Length + b.Length];
        Array.Copy(a, 0, result, 0, a.Length);
        Array.Copy(b, 0, result, a.Length, b.Length);
        return result;
    }

    public static byte[] Merge(byte[] a, byte[] b)
    {
        byte[] result = new byte[a.Length + b.Length];
        Buffer.BlockCopy(a, 0, result, 0, a.Length);
        Buffer.BlockCopy(b, 0, result, a.Length, b.Length);
        return result;
    }
}
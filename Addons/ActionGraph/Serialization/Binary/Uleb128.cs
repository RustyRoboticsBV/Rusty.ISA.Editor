using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for encoding integers in unsigned LEB128.
/// </summary>
internal static class Uleb128
{
    public static byte[] Encode(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "ULEB128 values must be non-negative.");

        List<byte> bytes = new(5);
        uint remaining = (uint)value;

        do
        {
            byte current = (byte)(remaining & 0x7F);
            remaining >>= 7;

            if (remaining != 0)
                current |= 0x80;

            bytes.Add(current);
        }
        while (remaining != 0);

        return bytes.ToArray();
    }

    public static int Decode(byte[] bytes, out int bytesRead)
    {
        if (bytes == null)
            throw new ArgumentNullException(nameof(bytes));

        uint result = 0;
        int shift = 0;

        for (int i = 0; i < bytes.Length; i++)
        {
            byte current = bytes[i];

            if (i == 4 && (current & 0xF0) != 0)
                throw new OverflowException("ULEB128 value exceeds Int32.");

            result |= (uint)(current & 0x7F) << shift;

            if ((current & 0x80) == 0)
            {
                bytesRead = i + 1;
                return checked((int)result);
            }

            shift += 7;
        }

        throw new FormatException("Incomplete ULEB128 sequence.");
    }
}
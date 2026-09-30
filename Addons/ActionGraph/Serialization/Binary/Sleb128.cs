using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for encoding integers in signed LEB128.
/// </summary>
internal static class Sleb128
{
    public static byte[] Encode(int value)
    {
        List<byte> bytes = new(5);
        int remaining = value;
        bool more;

        do
        {
            byte current = (byte)(remaining & 0x7F);
            remaining >>= 7;

            bool signBitSet = (current & 0x40) != 0;

            more = !((remaining == 0 && !signBitSet) || (remaining == -1 && signBitSet));
            if (more)
                current |= 0x80;

            bytes.Add(current);
        }
        while (more);

        return bytes.ToArray();
    }

    public static int Decode(byte[] bytes, out int bytesRead)
    {
        if (bytes == null)
            throw new ArgumentNullException(nameof(bytes));

        int result = 0;
        int shift = 0;

        for (int i = 0; i < bytes.Length; i++)
        {
            byte current = bytes[i];

            if (i == 4 && (current & 0xF0) != 0 && (current & 0xF0) != 0x70)
                throw new OverflowException("SLEB128 value exceeds Int32.");

            result |= (current & 0x7F) << shift;

            if ((current & 0x80) == 0)
            {
                if (shift < 32 && (current & 0x40) != 0)
                    result |= -1 << (shift + 7);

                bytesRead = i + 1;
                return result;
            }

            shift += 7;

            if (shift >= 32)
                throw new OverflowException("SLEB128 value exceeds Int32.");
        }

        throw new FormatException("Incomplete SLEB128 sequence.");
    }
}
namespace Rusty.ActionGraph.Serialization;

internal static class Bitmask
{
    /* Public methods. */
    public static byte SetBit(byte mask, int bit, bool value)
    {
        byte flag = (byte)(1 << bit);
        return (byte)(value ? mask | flag : mask & ~flag);
    }

    public static bool GetBit(byte mask, int bit) => (mask & (1 << bit)) != 0;
}
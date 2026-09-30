using System.Text;

namespace Rusty.ActionGraph.Serialization;

internal static class BinaryStringValue
{
    /* Public methods. */
    public static byte[] Encode(string str)
    {
        byte[] value = Encoding.ASCII.GetBytes(str ?? "");
        byte[] size = Uleb128.Encode(value.Length);

        return ArrayMerger.Merge(size, value);
    }

    public static string Decode(byte[] bytes)
    {
        int size = Uleb128.Decode(bytes, out int sizeBytes);
        return Encoding.ASCII.GetString(bytes, sizeBytes, size);
    }
}
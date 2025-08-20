using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A general string parsing / serializing utility.
/// </summary>
public static class StringUtility
{
    /* Public methods. */
    /// <summary>
    /// Convert an object to a string.
    /// </summary>
    public static string Serialize(object obj)
    {
        switch (obj)
        {
            case null:
                return "";
            case Vector2 float2:
                return $"({float2.X}, {float2.Y})";
            case Vector2I int2:
                return $"({int2.X}, {int2.Y})";
            case Vector3 float3:
                return $"({float3.X}, {float3.Y}), {float3.Z})";
            case Vector3I int3:
                return $"({int3.X}, {int3.Y}, {int3.Z})";
            case Vector4 float4:
                return $"({float4.X}, {float4.Y}, {float4.Z}, {float4.W})";
            case Vector4I int4:
                return $"({int4.X}, {int4.Y}, {int4.Z}, {int4.W})";
            case Color color:
                return '#' + color.ToHtml(color.A < 1f);
            default:
                return obj.ToString();
        }
    }

    /// <summary>
    /// Parse a string as an bool.
    /// </summary>
    public static bool ParseBool(string text)
    {
        if (bool.TryParse(text, out bool result))
            return result;
        return false;
    }

    /// <summary>
    /// Parse a string as an integer.
    /// </summary>
    public static int ParseInt(string text)
    {
        if (int.TryParse(text, out int result))
            return result;
        return 0;
    }

    /// <summary>
    /// Parse a string as an float.
    /// </summary>
    public static float ParseFloat(string text)
    {
        if (float.TryParse(text, out float result))
            return result;
        return 0f;
    }

    /// <summary>
    /// Parse a string as an char.
    /// </summary>
    public static char ParseChar(string text)
    {
        if (char.TryParse(text, out char result))
            return result;
        return ' ';
    }

    /// <summary>
    /// Parse a string as a color.
    /// </summary>
    public static Color ParseColor(string text)
    {
        try
        {
            return Color.FromHtml(text);
        }
        catch
        {
            return Colors.Transparent;
        }
    }
}
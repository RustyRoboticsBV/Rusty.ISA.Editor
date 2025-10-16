using Godot;
using Godot.Collections;

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
        if (obj == null)
            return "";

        if (obj is Color color)
            return '#' + color.ToHtml(color.A < 1f);

        if (obj is Array array)
        {
            string str = "";
            foreach (var entry in array)
            {
                if (str != "")
                    str += ", ";
                str += Serialize(entry);
            }
            return $"[{str}]";
        }

        if (obj is Dictionary dict)
        {
            string str = "";
            foreach (var entry in dict)
            {
                if (str != "")
                    str += ", ";
                str += $"\"{entry.Key}\": {Serialize(entry.Value)}";
            }
            return '{' + str + '}';
        }

        try
        {
            Variant variant = Variant.From(obj);
            return Json.Stringify(variant);
        }
        catch
        {
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
    /// Parse a string as a float.
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

    /// <summary>
    /// Parse a string as an int vector2.
    /// </summary>
    public static Vector2I ParseVector2I(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Vector2I)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as an int vector3.
    /// </summary>
    public static Vector3I ParseVector3I(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Vector3I)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as an int vector4.
    /// </summary>
    public static Vector4I ParseVector4I(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Vector4I)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as a float vector2.
    /// </summary>
    public static Vector2 ParseVector2(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Vector2)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as a float vector3.
    /// </summary>
    public static Vector3 ParseVector3(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Vector3)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as a float vector4.
    /// </summary>
    public static Vector4 ParseVector4(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Vector4)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as a quaternion.
    /// </summary>
    public static Quaternion ParseQuaternion(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Quaternion)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as a plane.
    /// </summary>
    public static Plane ParsePlane(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Plane)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as an int rectangle.
    /// </summary>
    public static Rect2I ParseRect2I(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Rect2I)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as a float rectangle.
    /// </summary>
    public static Rect2 ParseRect2(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Rect2)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as an axis-aligned bounding box.
    /// </summary>
    public static Aabb ParseAABB(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Aabb)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as a 2x3 matrix.
    /// </summary>
    public static Transform2D ParseTransform2D(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Transform2D)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as a 4x4 matrix.
    /// </summary>
    public static Transform3D ParseTransform3D(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Transform3D)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as a 3x3 matrix.
    /// </summary>
    public static Basis ParseBasis(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Basis)result;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// Parse a string as a dictionary.
    /// </summary>
    public static Dictionary ParseDictionary(string text)
    {
        Variant result = Json.ParseString(text);
        try
        {
            return (Dictionary)result;
        }
        catch
        {
            return new();
        }
    }
}
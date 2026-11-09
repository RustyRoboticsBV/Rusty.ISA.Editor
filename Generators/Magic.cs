/// <summary>
/// Magic values for binary programs.
/// </summary>
public static class Magic
{
    public static string formatName => "AGBP";
    public static string binaryHeader => "AGBP" + "\\0\\0\\0\\0";
    public static string extension => formatName.ToLowerInvariant();

    public static string version = "1.0";

    public static byte b0 => (byte)formatName[0];
    public static byte b1 => (byte)formatName[1];
    public static byte b2 => (byte)formatName[2];
    public static byte b3 => (byte)formatName[3];
    public static byte b4 => 0;
    public static byte b5 => 0;
    public static byte b6 => 0;
    public static byte b7 => 0;

    public static string h0 => $"{b0:X2}";
    public static string h1 => $"{b1:X2}";
    public static string h2 => $"{b2:X2}";
    public static string h3 => $"{b3:X2}";
    public static string h4 => $"{b4:X2}";
    public static string h5 => $"{b5:X2}";
    public static string h6 => $"{b6:X2}";
    public static string h7 => $"{b7:X2}";
}
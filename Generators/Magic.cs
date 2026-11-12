/// <summary>
/// Magic values for binary programs.
/// </summary>
public static class Magic
{
    public static string extension => "agp";

    public static string version = "1.0";

    public static byte b0 => 0xDF;
    public static byte b1 => 0xD8;
    public static byte b2 => 0x41;
    public static byte b3 => 0x47;
    public static byte b4 => 0x0D;
    public static byte b5 => 0x0A;
    public static byte b6 => 0x1A;
    public static byte b7 => 0x0A;

    public static string h0 => $"{b0:X2}";
    public static string h1 => $"{b1:X2}";
    public static string h2 => $"{b2:X2}";
    public static string h3 => $"{b3:X2}";
    public static string h4 => $"{b4:X2}";
    public static string h5 => $"{b5:X2}";
    public static string h6 => $"{b6:X2}";
    public static string h7 => $"{b7:X2}";
}
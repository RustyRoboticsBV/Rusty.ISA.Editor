public static class Magic
{
    public const char c0 = 'A';
    public const char c1 = 'G';
    public const char c2 = 'B';
    public const char c3 = 'P';
    public const char c4 = '\0';
    public const char c5 = '\0';
    public const char c6 = '\0';
    public const char c7 = '\0';

    public static byte b0 = (byte)c0;
    public static byte b1 = (byte)c1;
    public static byte b2 = (byte)c2;
    public static byte b3 = (byte)c3;
    public static byte b4 = (byte)c4;
    public static byte b5 = (byte)c5;
    public static byte b6 = (byte)c6;
    public static byte b7 = (byte)c7;

    public static string h0 => $"{b0:X2}";
    public static string h1 => $"{b1:X2}";
    public static string h2 => $"{b2:X2}";
    public static string h3 => $"{b3:X2}";
    public static string h4 => $"{b4:X2}";
    public static string h5 => $"{b5:X2}";
    public static string h6 => $"{b6:X2}";
    public static string h7 => $"{b7:X2}";
}
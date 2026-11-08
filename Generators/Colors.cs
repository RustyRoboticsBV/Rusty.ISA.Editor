using System;

public static class Colors
{
    public static string Pastel(int index, int count)
    {
        double hue = (index % count) * (360.0 / count);
        double saturation = 0.5;
        double value = 0.8;
        return Get(hue, value, saturation);
    }

    public static string Get(double hue, double value, double saturation)
    {
        double c = value * saturation;
        double x = c * (1 - Math.Abs((hue / 60.0) % 2 - 1));
        double m = value - c;

        double r = 0, g = 0, b = 0;

        if (hue < 60) (r, g, b) = (c, x, 0);
        else if (hue < 120) (r, g, b) = (x, c, 0);
        else if (hue < 180) (r, g, b) = (0, c, x);
        else if (hue < 240) (r, g, b) = (0, x, c);
        else if (hue < 300) (r, g, b) = (x, 0, c);
        else (r, g, b) = (c, 0, x);

        int R = (int)Math.Round((r + m) * 255);
        int G = (int)Math.Round((g + m) * 255);
        int B = (int)Math.Round((b + m) * 255);

        return $"{R:X2}{G:X2}{B:X2}";
    }

    public static string Generate(int index)
    {
        const double goldenAngle = 137.508;
        const double L = 0.70;
        const double C = 0.16;

        double hue = (index * goldenAngle) % 360.0;
        double radians = hue * Math.PI / 180.0;

        double a = C * Math.Cos(radians);
        double b = C * Math.Sin(radians);

        // OKLab -> linear RGB
        double l_ = L + 0.3963377774 * a + 0.2158037573 * b;
        double m_ = L - 0.1055613458 * a - 0.0638541728 * b;
        double s_ = L - 0.0894841775 * a - 1.2914855480 * b;

        double l = l_ * l_ * l_;
        double m = m_ * m_ * m_;
        double s = s_ * s_ * s_;

        double r = 4.0767416621 * l
                 - 3.3077115913 * m
                 + 0.2309699292 * s;

        double g = -1.2684380046 * l
                 + 2.6097574011 * m
                 - 0.3413193965 * s;

        double blue = -0.0041960863 * l
                    - 0.7034186147 * m
                    + 1.7076147010 * s;

        int R = ToByte(r);
        int G = ToByte(g);
        int B = ToByte(blue);

        return $"{R:X2}{G:X2}{B:X2}";
    }

    private static int ToByte(double value)
    {
        value = Math.Clamp(value, 0.0, 1.0);

        double srgb = value <= 0.0031308
            ? 12.92 * value
            : 1.055 * Math.Pow(value, 1.0 / 2.4) - 0.055;

        return (int)Math.Round(srgb * 255);
    }
}

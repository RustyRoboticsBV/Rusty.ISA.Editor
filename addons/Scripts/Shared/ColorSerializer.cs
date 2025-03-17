using Godot;

namespace Rusty.ISA.Importer
{
    /// <summary>
    /// Utility class that can serializer colors.
    /// </summary>
    public static class ColorSerializer
    {
        /// <summary>
        /// Serialize a color.
        /// </summary>
        public static string Serialize(Color color)
        {
            return $"#{color.ToHtml(true)}";
        }
    }
}

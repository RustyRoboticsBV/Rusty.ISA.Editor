using Godot;

namespace Rusty.ISA.Editor
{
    /// <summary>
    /// A logging utility class. It prints text both to the editor console, and to the in-app console.
    /// </summary>
    public static class Log
    {
        /* Public methods. */
        /// <summary>
        /// Print a message.
        /// </summary>
        public static void Message(string text)
        {
            GD.Print("\u25CF " + text);
        }

        /// <summary>
        /// Print a multi-line message.
        /// </summary>
        public static void Message(params object[] text)
        {
            Message(Combine(text));
        }

        /// <summary>
        /// Print a warning.
        /// </summary>
        public static void Warning(string text)
        {
            GD.PrintRich($"[color=#ffde66]\u25CF {text}[/color]");
        }

        /// <summary>
        /// Print a multi-line warning.
        /// </summary>
        public static void Warning(params object[] text)
        {
            Warning(Combine(text));
        }

        /// <summary>
        /// Print an error.
        /// </summary>
        public static void Error(string text)
        {
            GD.PrintErr(text);
        }

        /// <summary>
        /// Print a multi-line error.
        /// </summary>
        public static void Error(params object[] text)
        {
            Error(Combine(text));
        }

        /* Private methods. */
        private static string Combine(object[] objs)
        {
            string text = "";
            foreach (var item in objs)
            {
                if (text.Length > 0)
                    text += "\n";
                text += item != null ? item.ToString() : "";
            }
            return text;
        }
    }
}
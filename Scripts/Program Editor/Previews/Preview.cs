using Godot;

namespace Rusty.ISA.Editor
{
    public abstract class Preview<T> where T : Inspector
    {
        /* Public properties. */
        public T Inspector { get; private set; }

        /* Constructors. */
        public Preview(T inspector)
        {
            Inspector = inspector;
        }

        /* Public methods. */
        /// <summary>
        /// Generate a preview from an expression.
        /// </summary>
        public string Generate(string expression)
        {
            // If the expression was empty, use the default expression.
            if (expression == "")
                return Execute(GetDefault());

            // Parse expression string...
            for (int i = 0; i < expression.Length - 1; i++)
            {
                if (CheckKeyword("%", "%", expression, i, out string parameterID))
                    ParseParameter(ref expression, ref i, parameterID.Length + 2, parameterID);
                else if (CheckKeyword("[", "]", expression, i, out string ruleID))
                    ParseCompileRule(ref expression, ref i, ruleID.Length + 2, ruleID);
            }

            // Execute expression.
            return Execute(expression);
        }

        /* Protected methods. */
        protected abstract string GetDefault();
        protected abstract void ParseParameter(ref string expression, ref int startIndex, int length, string parameterID);
        protected abstract void ParseCompileRule(ref string expression, ref int startIndex, int length, string ruleID);

        protected static void Replace(ref string str, ref int startIndex, int length, string substr)
        {
            str = str.Substring(0, startIndex) + substr + str.Substring(startIndex + length);
            startIndex += substr.Length - 1;
        }

        protected static string MakeExpression(object obj)
        {
            return $"{'\"'}{obj}{'\"'}";
        }

        /* Private methods. */
        private static string Execute(string str)
        {
            GD.Print("Executing: " + str);
            Expression expression = new();
            if (expression.Parse(str) == Error.Ok)
            {
                return (string)expression.Execute();
            }
            else
                return $"bad_expression";
        }

        private static bool CheckKeyword(string startEscape, string endEscape, string expression, int index, out string keyword)
        {
            if (index < expression.Length - startEscape.Length - endEscape.Length
                && expression.Substring(index, startEscape.Length) == startEscape)
            {
                int end = expression.IndexOf(endEscape, index + startEscape.Length);

                if (end != -1)
                {
                    keyword = expression.Substring(index + startEscape.Length, end - index - startEscape.Length);
                    return true;
                }
            }

            keyword = "";
            return false;
        }
    }
}
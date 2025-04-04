#define RUSTY_ISA_EDITOR_PREVIEWS
using Godot;

namespace Rusty.ISA.Editor.Programs
{
    public abstract class Preview<T> where T : Inspector
    {
        /* Public properties. */
        public T Inspector { get; private set; }

        /* Private properties. */
        private static string SkeletonCode =>
            "extends Node;\n" +
            "\n" +
            "# %DEBUGNAME%\n" +
            "func eval() -> String:\n" +
            "\tvar _elements : Array[String] = [%ELEMENTS%];\n" +
            "\tvar result : String = \"\";\n" +
            "\t%IMPLEMENTATION%\n" +
            "\treturn result;";

        private static string Limit =>
            "\n" +
            "func limit(string : String, max_length : int) -> String:\n" +
            "\tif string.length() > max_length:\n" +
            "\t\treturn string.substr(0, max_length - 1) + String.chr(8230);\n" +
            "\telse:" +
            "\t\treturn string;";

        private string Implementation { get; set; } = "";
        private Node Node { get; set; }

        /* Constructors. */
        public Preview() : this(null, "") { }

        public Preview(T inspector, string implementation)
        {
            Inspector = inspector;
            Implementation = implementation;
        }

        /* Public methods. */
        /// <summary>
        /// Execute the generated eval method.
        /// </summary>
        public string Evaluate()
        {
#if !RUSTY_ISA_EDITOR_PREVIEWS
            return "PREVIEWS_HAVE_BEEN_DISABLED";
#else
            // Create node if necessary.
            if (Node == null)
                Node = CreateNode(Implementation);

            // Evaluate.
            if (Node != null)
                return (string)Node.Call("eval");
            else
                return "BAD_PREVIEW";
#endif
        }

        /* Protected methods. */
        /// <summary>
        /// Gets a debug name string.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetDebugName();
        /// <summary>
        /// Get the elements.
        /// </summary>
        protected virtual string GetElements()
        {
            return " ";
        }
        /// <summary>
        /// Get the default expression that is used in place of the empty string.
        /// </summary>
        protected abstract string GetDefaultExpression();
        /// <summary>
        /// Get the special keywords that can be used in this preview.
        /// </summary>
        protected virtual string[] GetSpecialKeywords()
        {
            return new string[0];
        }
        /// <summary>
        /// Parse a parameter keyword.
        /// </summary>
        protected abstract string ParseParameter(string parameterID);
        /// <summary>
        /// Parse a compile rule keyword.
        /// </summary>
        protected abstract string ParseCompileRule(string ruleID);
        /// <summary>
        /// Search the expression for special keywords and parse them.
        /// </summary>
        protected virtual string ParseSpecialKeyword(string keyword)
        {
            return GetSpecialKeywordError(keyword);
        }

        /// <summary>
        /// Make an expression.
        /// </summary>
        protected static string Make(string str)
        {
            return "\"" + str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n") + "\"";
        }

        protected static string GetError(string str)
        {
            return Make(str);
        }

        protected static string GetNullError(string str)
        {
            return GetError($"NULL({str})");
        }

        protected static string GetParameterError(string parameterID)
        {
            return GetError($"BAD_PARAMETER({parameterID})");
        }

        protected static string GetRuleError(string ruleID)
        {
            return GetError($"BAD_RULE({ruleID})");
        }

        protected static string GetSpecialKeywordError(string keyword)
        {
            return GetError($"BAD_SPECIAL_KEYWORD({keyword})");
        }

        /* Private methods. */
        /// <summary>
        /// Create an evaluation node.
        /// </summary>
        private Node CreateNode(string expression)
        {
            string inspectorType = Inspector != null ? Inspector.GetType().Name : "Node";

            // If the expression was empty, use the default expression.
            if (expression == "")
            {
                string defaultValue = GetDefaultExpression();
                if (defaultValue != "")
                    expression = "result = " + defaultValue;
                else
                    expression = "";
            }

            // If the expression was NOT empty...
            else
            {
                // Fix simple single-line expressions.
                if (!expression.Contains('\n') && !expression.StartsWith("return") && !expression.StartsWith("result"))
                {
                    expression = "result = " + expression;
                }

                // Add indentation.
                expression = expression.Replace("\n", "\n\t");

                // Parse expression keywords...
                string[] specialKeywords = GetSpecialKeywords();
                bool withinQuotes = false;
                for (int i = 0; i < expression.Length - 1; i++)
                {
                    if (expression[i] == '"' && (!withinQuotes || withinQuotes && i > 0 && expression[i - 1] != '\\'))
                        withinQuotes = !withinQuotes;
                    if (withinQuotes)
                        continue;
                    if (CheckKeyword("%", "%", expression, i, out string parameterID))
                    {
                        string parsed = ParseParameter(parameterID);
                        Replace(ref expression, ref i, parameterID.Length + 2, parsed);
                    }
                    else if (CheckKeyword("[", "]", expression, i, out string ruleID))
                    {
                        string parsed = ParseCompileRule(ruleID);
                        Replace(ref expression, ref i, ruleID.Length + 2, parsed);
                    }
                    else
                    {
                        foreach (string keyword in specialKeywords)
                        {
                            if (expression.Substring(i).StartsWith(keyword))
                            {
                                string parsed = ParseSpecialKeyword(keyword);
                                Replace(ref expression, ref i, keyword.Length, parsed);
                                break;
                            }
                        }
                    }
                }
            }

            // Create source code.
            string code = SkeletonCode
                .Replace("%INSPECTOR%", inspectorType)
                .Replace("%ELEMENTS%", GetElements())
                .Replace("%IMPLEMENTATION%", expression)
                .Replace("%DEBUGNAME%", GetDebugName());

            // Add limit method if necessary.
            if (code.Contains("limit("))
                code += Limit;

            // Create script.
            GDScript script = new();
            script.SourceCode = code;
            Error error = script.Reload();

            // Return instantiated node.
            if (error == Error.Ok)
                return (Node)script.New();
            else
            {
                GD.PrintErr("Bad preview expression: \n" + code);
                return null;
            }
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

        private static void Replace(ref string str, ref int startIndex, int length, string substr)
        {
            str = str.Substring(0, startIndex) + substr + str.Substring(startIndex + length);
            startIndex += substr.Length - 1;
        }
    }
}
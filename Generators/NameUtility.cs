using System.Text;

internal static class NameUtility
{
    public static string GetCodecName(string elementName)
    {
        return $"{CamelCase(elementName)}Codec";
    }

    public static string CamelCase(string snake_case)
    {
        StringBuilder sb = new();
        for (int i = 0; i < snake_case.Length; i++)
        {
            if (i == 0)
                sb.Append(snake_case[0].ToString().ToUpperInvariant());
            else if (i < snake_case.Length - 1 && snake_case[i] == '_')
            {
                sb.Append(snake_case[i + 1].ToString().ToUpperInvariant());
                i++;
            }
            else
                sb.Append(snake_case[i]);
        }
        string name = sb.ToString();
        if (name == "Id")
            return "ID";
        else
            return name;
    }
}
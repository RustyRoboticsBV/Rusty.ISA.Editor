public sealed class GroupInterfaceGenerator : Generator
{
    public string Generate(Schema schema)
    {

        CodeWriter w = new();
        w.Line(Notice);
        w.Line();
        Namespace("Rusty", "ActionGraph", "Serialization").Generate(w);
        w.Line();

        string generic = "ChildElementT";
        string key = Names.CamelCase(schema.SharedAttributes[0].Name);
        string parameter = Names.PascalCase(key);
        string code =
@$"internal interface IElementGroup<{generic}>
    where {generic} : Element
{{
    /// <summary>
    /// Get a child element of the return type with a specific {key}. Returns null if the child element cannot be found.
    /// </summary>
    public {generic} Find(string {parameter})
    {{
        Element _this = this as Element;
        for (int i = 0; i < _this.ChildCount; i++)
        {{
            if (_this.GetChild(i) is {generic} child && child.{key} == {parameter})
                return child;
        }}
        return null;
    }}
}}";
        w.Line(code);
        return w.ToString();
    }
}
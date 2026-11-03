public sealed class GroupInterfaceGenerator : Generator
{
    public string Generate(Schema schema)
    {
        string generic = "ChildElementT";
        string key = Names.CamelCase(schema.GlobalAttributes[0].Name);
        string parameter = Names.PascalCase(key);
        string code =
@$"{Notice}

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// An element that represents a group of child elements of some type.
/// </summary>
internal interface IElementGroup<{generic}>
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
        return code;
    }
}
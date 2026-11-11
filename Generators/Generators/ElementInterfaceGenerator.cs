public sealed class ElementInterfaceGenerator : Generator
{
    public string Generate(Schema schema, Abstract @abstract)
    {
        // Generate global attribute properties.
        bool needsGodot = false;
        CodeWriter shareds = new();
        shareds.Indent++;
        foreach (Attribute shared in @abstract.Attributes)
        {
            if (shared.ValueType == ValueType.Color)
                needsGodot = true;

            shareds.Line($"// <summary>");
            shareds.Line($"// {shared.Description}");
            shareds.Line($"// </summary>");
            shareds.Line($"public {Types.Get(shared.ValueType)} {Names.CamelCase(shared.Name)} {{ get; set; }}");
        }

        // Generate script.
        return $@"{Notice}{(needsGodot ? "\n\nusing Godot;" : "")}

namespace Rusty.ActionGraph.Serialization;

/// <summary>
///  {@abstract.Description}
/// </summary>
internal interface I{Names.ClassName(@abstract.Name)}
{{
{shareds}
}}";
    }
}
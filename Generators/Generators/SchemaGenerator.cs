public class SchemaGenerator : Generator
{
    public string Generate(Schema schema)
    {
        CodeWriter indexOfCases = new();
        indexOfCases.Indent = 2;
        foreach (Sealed @sealed in schema.Sealeds)
        {
            string name = Names.ClassName(@sealed.Name);
            indexOfCases.Line($"{name}.ELEMENT_NAME => {name}.ELEMENT_INDEX,");
        }

        CodeWriter nameOfCases = new();
        nameOfCases.Indent = 2;
        foreach (Sealed @sealed in schema.Sealeds)
        {
            string name = Names.ClassName(@sealed.Name);
            nameOfCases.Line($"{name}.ELEMENT_INDEX => {name}.ELEMENT_NAME,");
        }

        CodeWriter typeOfCases = new();
        typeOfCases.Indent = 2;
        foreach (Sealed @sealed in schema.Sealeds)
        {
            string name = Names.ClassName(@sealed.Name);
            typeOfCases.Line($"{name}.ELEMENT_INDEX => typeof({name}),");
        }

        return @$"{Notice}

using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// The ActionGraph element types.
/// </summary>
internal static class Elements
{{
    /* Public constants. */
    public const int ELEMENT_COUNT = {schema.Sealeds.Count};
    
    /* Public methods. */
    /// <summary>
    /// Get the index of an element, using its name.
    /// </summary>
    public static int IndexOfElement(string name) => name switch
    {{
{indexOfCases}
        _ => throw new ArgumentException($""Unknown element '{{name}}'."", nameof(name))
    }};

    /// <summary>
    /// Get the name of an element, using its index.
    /// </summary>
    public static string NameOfElement(int index) => index switch
    {{
{nameOfCases}
        _ => throw new ArgumentException($""Unknown element '{{index}}'."", nameof(index))
    }};

    /// <summary>
    /// Get the type of an element, using its index.
    /// </summary>
    public static Type TypeOfElement(int index) => index switch
    {{
{typeOfCases}
        _ => throw new ArgumentException($""Unknown element '{{index}}'."", nameof(index))
    }};
    
    /// <summary>
    /// Instantiate an element of some type, using its index.
    /// </summary>
    public static Element Instantiate(int index) => (Element)Activator.CreateInstance(TypeOfElement(index));
}}";
    }
}
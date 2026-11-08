public class ImHexPatternGenerator : Generator
{
    public string Generate(Schema schema)
    {
        string root = "LocalizedValue";

        CodeWriter globals = new();
        globals.Indent = 1;
        foreach (Attribute global in schema.GlobalAttributes)
        {
            WriteAttribute(global, globals);
        }

        CodeWriter elements = new();
        foreach (Sealed @sealed in schema.Sealeds)
        {
            elements.Line();
            elements.Line($"struct {Names.CamelCase(@sealed.Name)} : Element {{");

            elements.Indent = 1;
            foreach (Attribute attribute in @sealed.Attributes)
            {
                if (schema.HasGlobalAttribute(attribute))
                    continue;

                WriteAttribute(attribute, elements);
            }

            if (@sealed.Children.Count > 0)
            {
                elements.Line();
                elements.Line("uint child_count;");
                elements.Line("Element children[child_count];");
            }

            elements.Indent = 0;
            elements.Line("};");
        }

        return @$"{Notice}

import type.leb128;
import type.color;
import std.string;

using uint = type::uLEB128;
using int = type::sLEB128;
using color = type::RGBA8;

struct string {{
    uint length;
    char value[length];
}};

struct Element {{
    uint index;
{globals}
}};{elements}

{root} root @ 0x00;
";
    }

    private static void WriteAttribute(Attribute attribute, CodeWriter w)
    {
        string type = attribute.ValueType != ValueType.Ref
            ? attribute.ValueType.ToString().ToLowerInvariant()
            : ValueType.String.ToString().ToLowerInvariant();

        string name = attribute.Name != "from" ? attribute.Name : "_from";
        w.Line($"{type} {name};");
    }
}
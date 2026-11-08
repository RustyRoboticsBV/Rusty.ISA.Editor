public class ImHexPatternGenerator : Generator
{
    public string Generate(Schema schema)
    {
        CodeWriter cases = new();
        cases.Indent = 2;
        int index = 0;
        foreach (Sealed @sealed in schema.Sealeds)
        {
            string color = Colors.Pastel(index, schema.Sealeds.Count);
            cases.Line($"({index}): {{");
            cases.Indent++;
            foreach (Attribute attribute in @sealed.Attributes)
            {
                string type = attribute.ValueType != ValueType.Ref
                    ? attribute.ValueType.ToString().ToLowerInvariant()
                    : ValueType.String.ToString().ToLowerInvariant();

                string name = attribute.Name != "from" ? attribute.Name : "_from";

                cases.Line($"{type} {name} [[color(\"{color}\")]];");
            }

            if (@sealed.Children.Count > 0)
            {
                cases.Line();
                cases.Line($"uint child_count [[color(\"{color}\")]];");
                cases.Line("Element children[child_count];");
            }

            cases.Line();
            cases.Line($"std::core::set_display_name(this, \"{@sealed.Name}\");");

            cases.Indent--;
            cases.Line("}");
            index++;
        }

        return @$"{Notice}

#pragma name {Magic.c0}{Magic.c1}{Magic.c2}{Magic.c3}
#pragma description ActionGraph Binary Program
#pragma magic [ {Magic.h0} {Magic.h1} {Magic.h2} {Magic.h3} {Magic.h4} {Magic.h5} {Magic.h6} {Magic.h7} ] @ 0x00

import type.leb128;
import type.color;

using uint = type::uLEB128;
using int = type::sLEB128;
using color = type::RGBA8;

struct string {{
    uint length;
    char value[length];
}} [[single_color]];

// Base.
struct Element {{
    uint index [[color(""FFFFFF"")]];

    match (index) {{
{cases}
    }}
}};

// Entry point.
struct Root {{
    u8 magic_header[8];
    string version;
    Element element;
}};

Root root @ 0x00;
";
    }
}
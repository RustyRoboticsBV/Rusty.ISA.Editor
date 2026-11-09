public class ImHexPatternGenerator : Generator
{
    public string Generate(Schema schema)
    {
        CodeWriter cases = new();
        cases.Indent = 2;
        int index = 0;
        foreach (Sealed @sealed in schema.Sealeds)
        {
            string main = @sealed.Color;
            string pastel = Colors.Brighten(@sealed.Color, 0.3);
            string dark = Colors.Darken(@sealed.Color, 0.2);

            cases.Line($"({index}): {{");
            cases.Indent++;
            int aindex = 0;
            foreach (Attribute attribute in @sealed.Attributes)
            {
                string color = aindex % 2 == 0 ? main : pastel;
                switch (attribute.ValueType)
                {
                    case ValueType.Bool:
                        cases.Line($"bool {attribute.Name} [[color(\"{color}\")]];");
                        break;
                    case ValueType.Uint:
                        cases.Line($"uint {attribute.Name} [[color(\"{color}\")]];");
                        break;
                    case ValueType.Int:
                        cases.Line($"int {attribute.Name} [[color(\"{color}\")]];");
                        break;
                    case ValueType.String:
                    case ValueType.Ref:
                        cases.Line($"uint {attribute.Name}_length [[color(\"{color}\")]];");
                        cases.Line($"char {attribute.Name}_text[{attribute.Name}_length] [[color(\"{color}\")]];");
                        break;
                    case ValueType.Color:
                        cases.Line($"u8 {attribute.Name}_r [[color(\"{color}\")]];");
                        cases.Line($"u8 {attribute.Name}_g [[color(\"{color}\")]];");
                        cases.Line($"u8 {attribute.Name}_b [[color(\"{color}\")]];");
                        cases.Line($"u8 {attribute.Name}_a [[color(\"{color}\")]];");
                        break;
                }
                aindex++;
            }

            if (@sealed.Children.Count > 0)
            {
                cases.Line();
                cases.Line($"uint child_count [[color(\"{dark}\")]];");
                cases.Line("Element children[child_count];");
            }

            cases.Line();
            cases.Line($"std::core::set_display_name(this, \"{@sealed.Name}\");");

            cases.Indent--;
            cases.Line("}");
            index++;
        }

        return @$"{Notice}

#pragma name {Magic.formatName}
#pragma description ActionGraph Binary Program
#pragma magic [ {Magic.h0} {Magic.h1} {Magic.h2} {Magic.h3} {Magic.h4} {Magic.h5} {Magic.h6} {Magic.h7} ] @ 0x00

import type.leb128;
import std.core;

using uint = type::uLEB128;
using int = type::sLEB128;

// Base.
struct Element {{
    uint index [[color(""FFFFFF"")]];

    match (index) {{
{cases}
    }}
}};

// Entry point.
struct Root {{
    char magic_header[8] [[color(""000000"")]];
    uint version_length [[color(""111111"")]];
    char version[version_length] [[color(""111111"")]];
    Element element;
}};

Root root @ 0x00;
";
    }
}
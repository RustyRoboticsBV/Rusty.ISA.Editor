public class ElementClassGenerator : ClassGenerator
{
    public static string Generate(Schema schema, Element element)
    {
        CodeWriter w = new();

        /*// Header.
        w.Line(Notice);
        w.Line();
        w.Line("using System;");
        w.Line();
        w.Line(Namespace);
        w.Line();

        // Start of class.
        string inherit = element.Inherits != null ? NameUtility.GetCodecName(element.Inherits.Name) : "Codec";
        string group = CheckGroup(schema, element);
        string groupInterface = group != "none" ? $", ICodecGroup<{NameUtility.GetCodecName(group)}>" : "";

        if (element.Description != "")
            w.Summary(element.Description);
        w.OpenBlock($"internal sealed class {NameUtility.GetCodecName(element.Name)} : {inherit}{groupInterface}");

        // Properties.
        w.MultilineComment("Public properties.");

        // Attribute count.
        w.Line($"public override int AttributeCount => {element.Attributes.Count};");
        w.Line();

        // Attribute properties.
        foreach (Attribute attribute in element.Attributes)
        {
            string id = NameUtility.CamelCase(attribute.Name);
            string type = TypeUtility.GetType(attribute.ValueType);
            if (attribute.Description.Length > 0)
                w.Summary(attribute.Description);

            if (schema.HasSharedAttribute(attribute) || element.Inherits != null && element.Inherits.HasAttribute(attribute))
                w.Property(type, id, "override");
            else
                w.Property(type, id);
        }

        // Methods.
        w.Line();
        w.MultilineComment("Public methods.");

        // Attribute getter.
        w.OpenBlock("public override object GetAttribute(int index) => index switch");
        for (int i = 0; i < element.Attributes.Count; i++)
        {
            var attribute = element.Attributes[i];
            w.Line($"{i} => {NameUtility.CamelCase(attribute.Name)},");
        }
        w.Line($"_ => throw new ArgumentOutOfRangeException(nameof(index))");
        w.CloseBlock(";");
        w.Line();

        // Attribute setter.
        w.OpenBlock("public override void SetAttribute(int index, object value)");
        w.OpenBlock("switch (index)");
        for (int i = 0; i < element.Attributes.Count; i++)
        {
            var attribute = element.Attributes[i];
            string type = TypeUtility.GetType(attribute.ValueType);
            w.Line($"case {i}: {NameUtility.CamelCase(attribute.Name)} = ({type})value; break;");
        }
        w.Line($"default: throw new ArgumentOutOfRangeException(nameof(index));");
        w.CloseBlock();
        w.CloseBlock();
        w.Line();

        // Add child method.
        w.OpenBlock("public override void AddChild(Codec child)");

        if (element.Children.Count > 0)
        {
            w.Line("if (!(");
            for (int i = 0; i < element.Children.Count; i++)
            {
                if (i > 0 && i % 2 == 0)
                    w.IndentedLine();
                if (i > 0)
                    w.Append(" || ");
                w.Append($"child is {NameUtility.GetCodecName(element.Children[i])}");
            }
            w.Line("))");
            w.OpenBlock();
            w.Line("throw new ArgumentException(");
            w.IndentedLine("$\"Codec '{GetType().Name}' does not allow children of type '{child.GetType().Name}'.\",");
            w.IndentedLine("nameof(child)");
            w.Line(");");
            w.Line("Children.Add(child);");
        }
        else
            w.Line("throw new InvalidOperationException($\"Codec '{GetType().Name}' does not allow any children.\");");
        w.CloseBlock();

        // Find method.
        if (group != "none")
        {
            string returnType = NameUtility.GetCodecName(group);

            w.Line();
            w.OpenBlock($"public {returnType} FindChild(string id)");
            w.OpenBlock("for (int i = 0; i < ChildCount; i++)");
            w.Line($"if (Children[i] is {returnType} typed && typed.ID == id)");
            w.IndentedLine("return typed;");
            w.CloseBlock();
            w.Line("return null;");
            w.CloseBlock();
        }

        // End of class.
        w.CloseBlock(); */

        // Godot using.
        string str = w.ToString();
        if (str.Contains("Godot."))
            str = str
                .Replace("Godot.", "")
                .Replace("using System;", "using Godot;\nusing System;");

        return str;
    }

    private static string CheckGroup(Schema schema, Element element)
    {
        Abstract inherits = null;
        foreach (string child in element.Children)
        {
            Element found = schema.GetElement(child);
            if (inherits == null)
                inherits = found.Inherits;
            else if (inherits != found.Inherits)
                return "none";
        }
        return inherits?.Name ?? "none";
    }
}

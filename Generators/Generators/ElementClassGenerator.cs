using System;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

public class ElementClassGenerator : ClassGenerator
{
    public static string Generate(Schema schema, Element element)
    {
        bool needsGodot = false;

        // Class header.
        Class @class = new()
        {
            Access = AccessType.Internal,
            Modifier = ModifierType.Sealed,
            Name = Names.GetCodecName(element.Name)
        };

        if (element.Description != "")
            @class.Summary = element.Description;

        if (element.Inherits != null)
            @class.Base = Names.GetCodecName(element.Inherits.Name);
        else
            @class.Base = "Codec";

        string group = CheckGroup(schema, element);
        if (group != null)
            @class.Interfaces.Add($"ICodecGroup<{Names.GetCodecName(group)}>");


        // Properties.
        @class.Members.Add(new MultilineComment("Public properties."));

        // Attribute count property.
        @class.Property(AccessType.Public, ModifierType.Override, "int", "AttributeCount")
            .AddGetter((Return)(Int)element.Attributes.Count)
            .AddSummary("The number of attributes that this codec has.");

        // Attribute properties.
        if (element.Attributes.Count > 0)
            @class.Members.Add(new Empty());

        foreach (Attribute attribute in element.Attributes)
        {
            if (attribute.ValueType == ValueType.Color)
                needsGodot = true;

            // Create property.
            Property property = @class.Property(Types.Get(attribute.ValueType), Names.CamelCase(attribute.Name));

            // Attribute description.
            if (attribute.Description.Length > 0)
                property.Summary = attribute.Description;

            // Shared attribute?
            if (schema.HasSharedAttribute(attribute) || (element.Inherits?.HasAttribute(attribute) ?? false))
                property.Modifier = ModifierType.Override;

            // Add to class.
            @class.Members.Add(property);
        }


        // Methods.
        @class.Members.Add(new Empty());
        @class.Members.Add(new MultilineComment("Public methods."));

        // GetAttribute method.
        @class.Members.Add(GetAttribute(element));

        // SetAttribute method.
        @class.Members.Add(new Empty());
        @class.Members.Add(SetAttribute(element));

        // AddChild method.
        Block addChildBlock = new();
        if (element.Children.Count > 0)
        {
            StringBuilder sb = new();
            sb.Append("if (!(");
            for (int i = 0; i < element.Children.Count; i++)
            {
                if (i > 0 && i % 2 == 0)
                    sb.Append("\n  ");
                if (i > 0)
                    sb.Append(" || ");
                sb.Append($"child is {Names.GetCodecName(element.Children[i])}");
            }
            sb.Append("\n))");
            addChildBlock.Statements.Add((Code)sb.ToString());

            sb = sb.Clear();
            sb.Append("throw new ArgumentException(");
            sb.Append("\n    $\"Codec '{GetType().Name}' does not allow children of type '{child.GetType().Name}'.\",");
            sb.Append("\n    nameof(child)");
            sb.Append("\n);");
            sb.Append("\nChildren.Add(child);");
            addChildBlock.Statements.Add(new Block((Code)sb.ToString()));
        }
        else
            addChildBlock.Statements.Add(new Throw("InvalidOperationException", (IString)"Codec '{GetType().Name}' does not allow any children."));

        Method addChild = new()
        {
            Modifier = ModifierType.Override,
            ReturnType = "void",
            Name = "AddChild",
            Parameters = [new("Codec", "child")],
            Implementation = addChildBlock
        };

        @class.Members.Add(new Empty());
        @class.Members.Add(addChild);

        // FindChild method.
        if (group != null)
        {
            Id rt = Names.GetCodecName(group);
            For @for = new((Int)0, (Id)"ChildCount", (Return)(Id)"typed");
            Method findChild = new()
            {
                ReturnType = rt,
                Name = "FindChild",
                Parameters = [new("string", "id")],
                Implementation = @for
            };

            @class.Members.Add(new Empty());
            @class.Members.Add(findChild);
        }

        /*w.Line();
        w.OpenBlock("for (int i = 0; i < ChildCount; i++)");
        w.Line($"if (Children[i] is {returnType} typed && typed.ID == id)");
        w.IndentedLine("return typed;");
        w.CloseBlock();
        w.Line("return null;");
        w.CloseBlock();*/


        // Generate code.
        CodeWriter w = new();
        w.Line(Notice);
        w.Line();
        if (needsGodot)
            w.Line("using Godot;");
        w.Line("using System;");
        w.Line();
        w.Line(Namespace);
        w.Line();
        w.Line();
        @class.Generate(w);
        /*
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

        return w.ToString();
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
                return null;
        }
        return inherits?.Name;
    }

    private static Method GetAttribute(Element element)
    {
        // Implementation.
        Switch @switch = new((Id)"index");
        for (int i = 0; i < element.Attributes.Count; i++)
        {
            @switch.Case((Int)i, (Return)(Id)Names.CamelCase(element.Attributes[i].Name));
        }
        @switch.Default = new(new Throw("ArgumentOutOfRangeException", (Id)"nameof(index)"));

        // Method.
        return new Method(AccessType.Public, ModifierType.Override, "object", "GetAttribute")
            .Parameter("int", "index")
            .Implement(@switch);
    }

    private static Method SetAttribute(Element element)
    {
        // Implementation.
        Switch @switch = new((Id)"index");
        for (int i = 0; i < element.Attributes.Count; i++)
        {
            @switch.Case((Int)i, new Assign(
                    (Id)Names.CamelCase(element.Attributes[i].Name),
                    new Cast(Types.Get(element.Attributes[i].ValueType), (Id)"value")
                )
            );
        }
        @switch.Default = new(new Throw("ArgumentOutOfRangeException", (Id)"nameof(index)"));

        // Method.
        return new Method(AccessType.Public, ModifierType.Override, "void", "SetAttribute")
            .Parameter("int", "index")
            .Parameter("object", "value")
            .Implement(@switch);
    }
}

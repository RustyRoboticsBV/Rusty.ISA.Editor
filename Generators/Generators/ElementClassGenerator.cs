using System;
using System.Text;

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
            Name = NameUtility.GetCodecName(element.Name)
        };

        if (element.Description != "")
            @class.Summary = element.Description;

        if (element.Inherits != null)
            @class.Base = NameUtility.GetCodecName(element.Inherits.Name);
        else
            @class.Base = "Codec";

        string group = CheckGroup(schema, element);
        if (group != null)
            @class.Interfaces.Add($"ICodecGroup<{NameUtility.GetCodecName(group)}>");


        // Properties.
        @class.Members.Add(new MultilineComment("Public properties."));

        // Attribute count property.
        @class.Members.Add(new Property()
        {
            Modifier = ModifierType.Override,
            Type = "int",
            Name = "AttributeCount",
            Getter = new()
            {
                Implementation = (Return)(Int)element.Attributes.Count
            }
        });

        // Attribute properties.
        if (element.Attributes.Count > 0)
            @class.Members.Add(new Empty());

        foreach (Attribute attribute in element.Attributes)
        {
            if (attribute.ValueType == ValueType.Color)
                needsGodot = true;

            // Create property.
            Property property = new()
            {
                Name = NameUtility.CamelCase(attribute.Name),
                Type = TypeUtility.GetType(attribute.ValueType),
                Getter = new(),
                Setter = new()
            };

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
        Switch getterSwitch = new((Id)"index");
        for (int i = 0; i < element.Attributes.Count; i++)
        {
            getterSwitch.Cases.Add(new Case((Int)i, (Return)(Id)NameUtility.CamelCase(element.Attributes[i].Name)));
        }
        getterSwitch.Default = new(new Throw("ArgumentOutOfRangeException", (Id)"nameof(index)"));

        Method getter = new()
        {
            Modifier = ModifierType.Override,
            ReturnType = "object",
            Name = "GetAttribute",
            Parameters = [new("int", "index")],
            Implementation = getterSwitch
        };

        @class.Members.Add(getter);

        // SetAttribute method.
        Switch setterSwitch = new((Id)"index");
        for (int i = 0; i < element.Attributes.Count; i++)
        {
            setterSwitch.Cases.Add(new Case(
                (Int)i,
                new Assign(
                    (Id)NameUtility.CamelCase(element.Attributes[i].Name),
                    new Cast(TypeUtility.GetType(element.Attributes[i].ValueType), (Id)"value")
                )
            ));
        }
        setterSwitch.Default = new(new Throw("ArgumentOutOfRangeException", (Id)"nameof(index)"));

        Method setter = new()
        {
            Modifier = ModifierType.Override,
            ReturnType = "void",
            Name = "SetAttribute",
            Parameters = [new("int", "index"), new("object", "value")],
            Implementation = setterSwitch
        };

        @class.Members.Add(new Empty());
        @class.Members.Add(setter);

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
                sb.Append($"child is {NameUtility.GetCodecName(element.Children[i])}");
            }
            sb.Append("\n))");
            addChildBlock.Statements.Add((Code)sb.ToString());

            sb.Clear();
            sb.Append("throw new ArgumentException(");
            sb.Append("\n    $\"Codec '{GetType().Name}' does not allow children of type '{child.GetType().Name}'.\",");
            sb.Append("\n    nameof(child)");
            sb.Append("\n);");
            sb.Append("\nChildren.Add(child);");
            addChildBlock.Statements.Add((Code)sb.ToString());
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
}

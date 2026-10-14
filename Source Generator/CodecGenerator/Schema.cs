using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

public enum ValueType { Bool, Uint, Int, Color, String, Ref }

public class Attribute
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ValueType ValueType { get; set; }
    public string RefType { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(ValueType);
        if (ValueType == ValueType.Ref)
        {
            sb.Append(" (");
            sb.Append(RefType);
            sb.Append(")");
        }
        sb.Append(' ');
        sb.Append(Name);
        if (Description != "")
        {
            sb.Append("    // ");
            sb.Append(Description);
        }
        return sb.ToString();
    }
}

public class Element
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Inherits { get; set; }
    public List<Attribute> Attributes { get; set; } = new();
    public List<string> Children { get; set; } = new();
    public bool MultipleChildren { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(Name);
        if (Inherits != "none")
        {
            sb.Append(" : ");
            sb.Append(Inherits);
        }
        if (Description != "")
        {
            sb.Append("  // ");
            sb.Append(Description);
        }

        sb.Append("\nAttributes:");
        foreach (var attribute in Attributes)
        {
            sb.Append("\n- ");
            sb.Append(attribute.ToString());
        }

        if (Children.Count > 0)
        {
            sb.Append("\n");
            sb.Append("Children:");
            foreach (var child in Children)
            {
                sb.Append("\n- ");
                sb.Append(child);
            }
            if (MultipleChildren)
            {
                sb.Append("\n");
                sb.Append("Multiple children: ");
                sb.Append(MultipleChildren);
            }
        }

        return sb.ToString();
    }
}

public class Schema
{
    public List<Element> Elements { get; } = new();

    public Schema(string json)
    {
        JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;
        JsonElement version = root.GetProperty("schema_version");
        JsonElement abstracts = root.GetProperty("abstract_types");
        JsonElement elements = root.GetProperty("element_types");

        foreach (var obj in elements.EnumerateObject())
        {
            Element element = new();
            element.Name = obj.Name;
            element.Description = obj.Value.GetProperty("description").GetString();
            element.Inherits = obj.Value.GetProperty("inherits_abstract").GetString();

            JsonElement attributes = obj.Value.GetProperty("attributes");
            foreach (var attr in attributes.EnumerateObject())
            {
                Attribute attribute = new();
                attribute.Name = attr.Name;
                attribute.Description = attr.Value.GetProperty("description").GetString();
                attribute.ValueType = Enum.Parse<ValueType>(attr.Value.GetProperty("value_type").GetString(), true);
                if (attribute.ValueType == ValueType.Ref)
                {
                    attribute.RefType = attr.Value.GetProperty("ref_type").GetString();
                }
                element.Attributes.Add(attribute);
            }

            JsonElement children = obj.Value.GetProperty("allowed_children");
            for (int i = 0; i < children.GetArrayLength(); i++)
            {
                element.Children.Add(children[i].GetString());
            }
            if (element.Children.Count > 0)
                element.MultipleChildren = obj.Value.GetProperty("allow_multiple_children").GetBoolean();

            Elements.Add(element);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        foreach (var element in Elements)
        {
            if (sb.Length > 0)
                sb.Append("\n\n");
            sb.Append(element.ToString());
        }
        return sb.ToString();
    }

    public Element Get(string name)
    {
        foreach (Element element in Elements)
        {
            if (element.Name == name)
                return element;
        }
        return null;
    }
}

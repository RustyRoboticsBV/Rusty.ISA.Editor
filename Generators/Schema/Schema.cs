using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

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

public class SharedAttribute
{
    public string Name { get; set; }
    public ValueType ValueType { get; set; }

    public SharedAttribute(Attribute attribute)
    {
        Name = attribute.Name;
        ValueType = attribute.ValueType;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(ValueType);
        sb.Append(' ');
        sb.Append(Name);
        return sb.ToString();
    }
}

public class Element
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Abstract Inherits { get; set; }
    public List<Attribute> Attributes { get; set; } = new();
    public List<string> Children { get; set; } = new();
    public bool MultipleChildren { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(Name);
        if (Inherits != null)
        {
            sb.Append(" : ");
            sb.Append(Inherits.Name);
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

public class Abstract
{
    public string Name { get; set; }
    public List<SharedAttribute> Attributes { get; } = new();

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(Name);

        sb.Append("\nAttributes:");
        foreach (var attribute in Attributes)
        {
            sb.Append("\n- ");
            sb.Append(attribute.ToString());
        }
        return sb.ToString();
    }

    public bool HasAttribute(Attribute attribute)
    {
        foreach (var shared in Attributes)
        {
            if (shared.Name == attribute.Name && shared.ValueType == attribute.ValueType)
                return true;
        }
        return false;
    }
}

public class Schema
{
    public string Version {get; set; }
    public List<Abstract> Abstracts { get; } = new();
    public List<Element> Elements { get; } = new();
    public List<SharedAttribute> SharedAttributes { get; } = new();

    public Schema(string json)
    {
        JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;

        // Parse schema version.
        Version = root.GetProperty("schema_version").GetString();

        // Parse abstract base classes.
        JsonElement abstracts = root.GetProperty("abstract_types");
        foreach (var obj in abstracts.EnumerateArray())
        {
            Abstract @abstract = new();
            @abstract.Name = obj.GetString();
            Abstracts.Add(@abstract);
        }

        // Parse elements.
        JsonElement elements = root.GetProperty("element_types");
        foreach (var obj in elements.EnumerateObject())
        {
            Element element = new();
            element.Name = obj.Name;
            element.Description = obj.Value.GetProperty("description").GetString();
            element.Inherits = GetAbstract(obj.Value.GetProperty("inherits_abstract").GetString());

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

        // Derive shared attributes.
        var shareds = GetSharedAttributes(null);
        foreach (var shared in shareds)
        {
            SharedAttributes.Add(shared);
        }

        foreach (var @abstract in Abstracts)
        {
            shareds = GetSharedAttributes(@abstract);
            foreach (var shared in shareds)
            {
                @abstract.Attributes.Add(shared);
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append("Shared attributes:");
        for (int i = 0; i < SharedAttributes.Count; i++)
        {
            sb.Append("\n- ");
            sb.Append(SharedAttributes[i].ToString());
        }
        sb.Append("\n");
        sb.Append("\n");
        sb.Append("\n");

        sb.Append("Abstracts:");
        for (int i = 0; i < Abstracts.Count; i++)
        {
            sb.Append("\n\n");
            sb.Append(Abstracts[i].ToString());
        }
        sb.Append("\n");
        sb.Append("\n");
        sb.Append("\n");

        sb.Append("Elements:");
        for (int i = 0; i < Elements.Count; i++)
        {
            sb.Append("\n\n");
            sb.Append(Elements[i].ToString());
        }

        return sb.ToString();
    }

    public Abstract GetAbstract(string name)
    {
        foreach (Abstract inherit in Abstracts)
        {
            if (inherit.Name == name)
                return inherit;
        }
        return null;
    }

    public Element GetElement(string name)
    {
        foreach (Element element in Elements)
        {
            if (element.Name == name)
                return element;
        }
        return null;
    }

    public bool HasSharedAttribute(Attribute attribute)
    {
        foreach (SharedAttribute shared in SharedAttributes)
        {
            if (shared.Name == attribute.Name && shared.ValueType == attribute.ValueType)
                return true;
        }
        return false;
    }

    public bool HasSharedAttribute(SharedAttribute shared)
    {
        foreach (SharedAttribute globalShared in SharedAttributes)
        {
            if (globalShared.Name == shared.Name && globalShared.ValueType == shared.ValueType)
                return true;
        }
        return false;
    }

    /* Private methods. */
    private HashSet<SharedAttribute> GetSharedAttributes(Abstract abstractType)
    {
        HashSet<SharedAttribute> result = null;

        foreach (Element element in Elements)
        {
            if (element.Inherits != abstractType)
                continue;

            if (result == null)
            {
                result = new();
                foreach (Attribute attribute in element.Attributes)
                {
                    result.Add(new(attribute));
                }
            }

            else
            {
                HashSet<SharedAttribute> buffer = new();
                foreach (SharedAttribute shared in result)
                {
                    foreach (Attribute attribute in element.Attributes)
                    {
                        if (attribute.Name == shared.Name && attribute.ValueType == shared.ValueType)
                            buffer.Add(shared);
                    }
                }
                result = buffer;
            }
        }

        return result;
    }
}

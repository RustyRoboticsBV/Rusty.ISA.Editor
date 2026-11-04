using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

public class Schema
{
    /* Public properties. */
    public string Version {get; set; }
    public List<Abstract> Abstracts { get; } = new();
    public List<Sealed> Sealeds { get; } = new();
    public List<Attribute> GlobalAttributes { get; } = new();

    /* Constructors. */
    public Schema(string json)
    {
        JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;

        // Parse schema version.
        Version = root.GetProperty("schema_version").GetString();

        // Parse abstract base classes.
        JsonElement abstracts = root.GetProperty("abstract_types");
        foreach (var obj in abstracts.EnumerateObject())
        {
            Abstract @abstract = new();
            @abstract.Name = obj.Name;
            @abstract.Description = obj.Value.GetProperty("description").GetString();
            Abstracts.Add(@abstract);
        }

        // Parse elements.
        Dictionary<Attribute, string> refTypes = new();

        JsonElement elements = root.GetProperty("element_types");
        foreach (var obj in elements.EnumerateObject())
        {
            Sealed element = new();
            element.Name = obj.Name;
            element.Description = obj.Value.GetProperty("description").GetString();
            element.Inherits = GetAbstract(obj.Value.GetProperty("inherits_abstract").GetString());

            // Parse attributes.
            JsonElement attributes = obj.Value.GetProperty("attributes");
            foreach (var attr in attributes.EnumerateObject())
            {
                Attribute attribute = new();
                attribute.Name = attr.Name;
                attribute.Description = attr.Value.GetProperty("description").GetString();
                attribute.ValueType = Enum.Parse<ValueType>(attr.Value.GetProperty("value_type").GetString(), true);
                if (attribute.ValueType == ValueType.Ref)
                    refTypes[attribute] = attr.Value.GetProperty("ref_type").GetString();
                element.Attributes.Add(attribute);
            }

            // Parse children.
            JsonElement children = obj.Value.GetProperty("allowed_children");
            for (int i = 0; i < children.GetArrayLength(); i++)
            {
                element.Children.Add(children[i].GetString());
            }
            if (element.Children.Count > 0)
                element.MultipleChildren = obj.Value.GetProperty("allow_multiple_children").GetBoolean();

            Sealeds.Add(element);
        }

        // Resolve ref types.
        foreach (var (attribute, typeName) in refTypes)
        {
            attribute.RefType = GetElement(typeName);
        }

        // Derive global & shared attributes.
        var shareds = GetSharedAttributes(null);
        foreach (Attribute shared in shareds)
        {
            GlobalAttributes.Add(shared);
        }

        foreach (Abstract @abstract in Abstracts)
        {
            shareds = GetSharedAttributes(@abstract);
            foreach (var shared in shareds)
            {
                @abstract.Attributes.Add(shared);
            }
        }

        // Finalize descriptions.
        foreach (Attribute shared in GlobalAttributes)
        {
            shared.Description = ExpandDescription(shared.Description, "element", null);
        }

        foreach (Abstract @abstract in Abstracts)
        {
            foreach (Attribute shared in @abstract.Attributes)
            {
                shared.Description = ExpandDescription(shared.Description, @abstract.Name, shared.RefType?.Name);
            }
        }

        foreach (Sealed element in Sealeds)
        {
            foreach (Attribute attribute in element.Attributes)
            {
                attribute.Description = ExpandDescription(attribute.Description, element.Name, attribute.RefType?.Name);
            }
        }
    }

    /* Public methods. */
    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append("Global attributes:");
        for (int i = 0; i < GlobalAttributes.Count; i++)
        {
            sb.Append("\n- ");
            sb.Append(GlobalAttributes[i].ToString());
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
        for (int i = 0; i < Sealeds.Count; i++)
        {
            sb.Append("\n\n");
            sb.Append(Sealeds[i].ToString());
        }

        return sb.ToString();
    }

    public Element GetElement(string name)
    {
        Abstract @asbtract = GetAbstract(name);
        if (@asbtract != null)
            return @asbtract;
        else
            return GetSealed(name);
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

    public Sealed GetSealed(string name)
    {
        foreach (Sealed element in Sealeds)
        {
            if (element.Name == name)
                return element;
        }
        return null;
    }

    public bool HasGlobalAttribute(Attribute attribute)
    {
        foreach (Attribute global in GlobalAttributes)
        {
            if (global.Name == attribute.Name && global.ValueType == attribute.ValueType)
                return true;
        }
        return false;
    }

    /* Private methods. */
    private HashSet<Attribute> GetSharedAttributes(Abstract abstractType)
    {
        Dictionary<Attribute, List<Attribute>> sharedSources = new();
        HashSet<Attribute> result = null;

        foreach (Sealed element in Sealeds)
        {
            // Skip this element if it does not inherit the abstract type.
            if (abstractType != null && element.Inherits != abstractType)
                continue;

            // First element that inherits the abstract element: add all attributes.
            // We will remove attributes not shared by all inheritors on subsequent loops.
            if (result == null)
            {
                result = new();
                foreach (Attribute attribute in element.Attributes)
                {
                    Attribute shared = new();
                    shared.Name = attribute.Name;
                    shared.ValueType = attribute.ValueType;
                    shared.RefType = attribute.RefType;
                    result.Add(shared);
                    sharedSources[shared] = [attribute];
                }
            }

            // Subsequent elements that inherit the abstract element: remove attributes that it does not share.
            else
            {
                HashSet<Attribute> buffer = new();
                foreach (Attribute shared in result)
                {
                    foreach (Attribute attribute in element.Attributes)
                    {
                        if (attribute.Name == shared.Name && attribute.ValueType == shared.ValueType)
                        {
                            buffer.Add(shared);

                            if (shared.RefType != attribute.RefType && shared.RefType != attribute.RefType?.GetBase())
                            {
                                if (shared.RefType.GetBase() == attribute.RefType.GetBase())
                                    shared.RefType = shared.RefType.GetBase();
                                else
                                    shared.RefType = null;
                            }
                        }
                    }
                }
                result = buffer;
            }
        }

        // Resolve descriptions.
        foreach (var (shared, sources) in sharedSources)
        {
            shared.Description = $"The {Names.Prose(shared.Name)} of this <this>.";

            string desc = null;
            foreach (Attribute source in sources)
            {
                if (desc == null)
                    desc = source.Description;
                else if (desc != source.Description)
                {
                    desc = null;
                    break;
                }
            }
            if (desc != null)
                shared.Description = desc;
        }

        return result;
    }

    private static string ExpandDescription(string desc, string @this, string refType)
    {
        desc = desc.Replace("<this>", Names.Prose(@this));
        if (refType != null)
            desc = desc.Replace("<ref_type>", Names.Prose(refType));
        return desc;
    }
}

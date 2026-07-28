using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A codec node.
/// </summary>
public abstract class Codec
{
    /* Constants. */
    public const string Checksum = "csum";
    public const string ID = "id";
    public const string Value = "value";
    public const string Exec = "exec";
    public const string Localizable = "loc";
    public const string Type = "type";
    public const string X = "x";
    public const string Y = "y";
    public const string Width = "width";
    public const string Height = "height";
    public const string Member = "member";
    public const string Start = "start";
    public const string Text = "text";
    public const string Color = "color";
    public const string Index = "index";
    public const string HideDefault = "hidedf";
    public const string From = "from";
    public const string Port = "port";
    public const string To = "to";

    /* Public properties. */
    public abstract string Tag { get; }
    public string InnerText { get; set; } = "";
    public List<Codec> Children { get; } = new();
    public Dictionary<string, string> Attributes { get; } = new();

    /* Protected properties. */
    protected virtual HashSet<string> AllowedChildren { get; } = new();
    protected virtual HashSet<string> AllowedAttributes { get; } = new();

    protected static Dictionary<string, Type> Codecs { get; } = new();

    /* Constructors. */
    static Codec()
    {
        Register<FileCodec>(FileCodec.TAG);

        // Metadata.
        Register<MetaCodec>(MetaCodec.TAG);

        // Schema.
        Register<IdefCodec>(IdefCodec.TAG);
        Register<PdefCodec>(PdefCodec.TAG);

        Register<NdefCodec>(NdefCodec.TAG);

        Register<FdefCodec>(FdefCodec.TAG);
        Register<OdefCodec>(OdefCodec.TAG);
        Register<CdefCodec>(CdefCodec.TAG);
        Register<TdefCodec>(TdefCodec.TAG);
        Register<LdefCodec>(LdefCodec.TAG);

        Register<VadefCodec>(VadefCodec.TAG);
        Register<OadefCodec>(OadefCodec.TAG);

        // Graph.
        Register<NodeCodec>(NodeCodec.TAG);
        Register<JointCodec>(JointCodec.TAG);
        Register<FrameCodec>(FrameCodec.TAG);
        Register<MemoCodec>(MemoCodec.TAG);

        Register<EdgeCodec>(EdgeCodec.TAG);

        Register<FormCodec>(FormCodec.TAG);
        Register<OptionCodec>(OptionCodec.TAG);
        Register<ChoiceCodec>(ChoiceCodec.TAG);
        Register<TupleCodec>(TupleCodec.TAG);
        Register<ListCodec>(ListCodec.TAG);

        Register<ArgCodec>(ArgCodec.TAG);
        Register<OutCodec>(OutCodec.TAG);
    }

    public Codec() { }

    public Codec(XmlNode xml)
    {
        if (xml.ChildNodes.Count == 0)
            InnerText = "";
        else if (xml.ChildNodes.Count == 1 && xml.ChildNodes[0] is XmlText)
            InnerText = xml.InnerText;
        else
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlComment)
                    continue;

                string childTag = child.Name;
                if (!Codecs.ContainsKey(childTag))
                    throw new InvalidOperationException($"Unknown child type '{childTag}'.");
                if (!AllowedChildren.Contains(childTag))
                    throw new InvalidOperationException($"Codec '{Tag}' cannot have a child of type '{child.Name}'.");
                AddChild(Instantiate(Codecs[childTag], child));
            }
        }

        foreach (XmlAttribute attribute in xml.Attributes)
        {
            if (!AllowedAttributes.Contains(attribute.Name))
                throw new InvalidOperationException($"Codec '{Tag}' cannot have an attribute of type '{attribute.Name}'.");
            Attributes.TryAdd(attribute.Name, attribute.Value);
        }
    }

    /* Public methods. */
    /// <summary>
    /// Return the string representation of this codec.
    /// </summary>
    public override string ToString()
    {
        StringBuilder sb = new();
        AppendToString(sb, "", true, true, true);
        return sb.ToString();
    }

    /// <summary>
    /// Return the string representation of this codec.
    /// </summary>
    public string ToString(bool omitChildren)
    {
        if (!omitChildren)
            return ToString();

        StringBuilder sb = new();
        AppendToString(sb, "", true, true, false);
        return sb.ToString();
    }

    // TODO: remove by turning giving the properties a public or internal getter.
    public bool AllowsChild(string type) => AllowedChildren.Contains(type);
    public bool AllowsAttribute(string name) => AllowedAttributes.Contains(name);

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static Codec Load(XmlNode xml)
    {
        if (Codecs.TryGetValue(xml.Name, out Type type))
            return Instantiate(type, xml);
        else
            throw new InvalidOperationException($"Unknown XML child tag '{xml.Name}'.");
    }

    /// <summary>
    /// Set an attribute's value.
    /// </summary>
    public void SetAttribute(string name, string value)
    {
        Attributes[name] = value;
    }

    /// <summary>
    /// Get an attribute's value. Returns "" if the attribute could not be found.
    /// </summary>
    public string GetAttribute(string name)
    {
        if (Attributes.TryGetValue(name, out var attribute))
            return attribute;
        return "";
    }

    /// <summary>
    /// Add a node of some type.
    /// </summary>
    public void AddChild(Codec node)
    {
        Children.Add(node);
    }

    /// <summary>
    /// Get the first child node with some tag. Returns null if the child does not exist.
    /// </summary>
    public T GetFirstChild<T>()
        where T : Codec
    {
        foreach (Codec child in Children)
        {
            if (child is T typed)
                return typed;
        }
        return null;
    }

    /// <summary>
    /// Find the first child codec with a specific attribute value.
    /// </summary>
    public Codec FindChildWithAttribute(string attributeName, string attributeValue)
    {
        foreach (Codec child in Children)
        {
            if (child.GetAttribute(attributeName) == attributeValue)
                return child;
        }
        return null;
    }

    /* Protected methods. */
    /// <summary>
    /// Instantiate a codec node from a type and XML node.
    /// </summary>
    protected static Codec Instantiate(Type type, XmlNode xml)
    {
        if (!type.IsAssignableTo(typeof(Codec)))
            throw new InvalidCastException($"Type {type.Name} is not a codec name.");

        ConstructorInfo ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, [typeof(XmlNode)]);
        if (ctor == null)
            throw new NullReferenceException($"Type {type.Name} has no constructor that takes an {nameof(XmlNode)} argument.");

        return ctor.Invoke([xml]) as Codec;
    }

    /* Private methods. */
    private void AppendToString(StringBuilder sb, string prefix, bool last, bool root, bool recurse)
    {
        if (!root)
        {
            sb.Append(prefix);
            sb.Append(last ? "\u2514\u2500" : "\u251C\u2500");
        }

        sb.Append(Tag);

        if (Attributes.Count > 0)
        {
            sb.Append(" {");

            bool first = true;
            foreach (var attribute in Attributes)
            {
                if (!first)
                    sb.Append(", ");

                sb.Append(attribute.Key);
                sb.Append("=\"");
                sb.Append(attribute.Value);
                sb.Append('"');

                first = false;
            }

            sb.Append('}');
        }

        if (!string.IsNullOrEmpty(InnerText))
        {
            sb.Append(" : \"");
            sb.Append(InnerText);
            sb.Append('"');
        }

        if (recurse)
        {
            sb.AppendLine();

            string childPrefix = root
                ? ""
                : prefix + (last ? "  " : "\u2502 ");

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].AppendToString(sb, childPrefix, i == Children.Count - 1, false, recurse);
            }
        }
        else
        {
            if (Children.Count > 0)
                sb.Append("...");
        }
    }

    private static void Register<T>(string tag)
    {
        Codecs.Add(tag, typeof(T));
    }
}
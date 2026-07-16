using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A codec node.
/// </summary>
public abstract class Codec
{
    /* Public properties. */
    public string InnerText { get; set; } = "";
    public Dictionary<string, List<Codec>> Children { get; } = new();
    public Dictionary<string, string> Attributes { get; } = new();

    /* Protected properties. */
    protected abstract string Tag { get; }
    protected virtual HashSet<string> AllowedChildren { get; } = new();
    protected virtual HashSet<string> AllowedAttributes { get; } = new();

    protected static Dictionary<string, Type> Codecs { get; } = new();
    protected static Dictionary<Type, string> Tags { get; } = new();

    /* Constructors. */
    static Codec()
    {
        Register<FileCodec>(FileCodec.TAG);

        // Metadata.
        Register<MetaCodec>(MetaCodec.TAG);
        Register<DataCodec>(DataCodec.TAG);
        Register<CheckCodec>(CheckCodec.TAG);

        // Schema.
        Register<SchemaCodec>(SchemaCodec.TAG);

        Register<InstrsCodec>(InstrsCodec.TAG);

        Register<IdefCodec>(IdefCodec.TAG);
        Register<PdefCodec>(PdefCodec.TAG);
        Register<ExecCodec>(ExecCodec.TAG);

        Register<NodesCodec>(NodesCodec.TAG);

        Register<NdefCodec>(NdefCodec.TAG);

        Register<FdefCodec>(FdefCodec.TAG);
        Register<OdefCodec>(OdefCodec.TAG);
        Register<CdefCodec>(CdefCodec.TAG);
        Register<TdefCodec>(TdefCodec.TAG);
        Register<LdefCodec>(LdefCodec.TAG);

        Register<VadefCodec>(VadefCodec.TAG);
        Register<OadefCodec>(OadefCodec.TAG);

        // Graph.
        Register<GraphCodec>(GraphCodec.TAG);

        Register<ElemsCodec>(ElemsCodec.TAG);

        Register<NodeCodec>(NodeCodec.TAG);
        Register<JointCodec>(JointCodec.TAG);
        Register<FrameCodec>(FrameCodec.TAG);
        Register<MemoCodec>(MemoCodec.TAG);

        Register<XCodec>(XCodec.TAG);
        Register<YCodec>(YCodec.TAG);
        Register<WidthCodec>(WidthCodec.TAG);
        Register<HeightCodec>(HeightCodec.TAG);

        Register<MemberCodec>(MemberCodec.TAG);
        Register<StartCodec>(StartCodec.TAG);
        Register<TextCodec>(TextCodec.TAG);
        Register<ColorCodec>(ColorCodec.TAG);

        Register<FormCodec>(FormCodec.TAG);
        Register<ArgCodec>(ArgCodec.TAG);
        Register<OptionCodec>(OptionCodec.TAG);
        Register<ChoiceCodec>(ChoiceCodec.TAG);
        Register<TupleCodec>(TupleCodec.TAG);
        Register<ListCodec>(ListCodec.TAG);

        Register<EdgesCodec>(EdgesCodec.TAG);

        Register<EdgeCodec>(EdgeCodec.TAG);

        Register<FromCodec>(FromCodec.TAG);
        Register<PortCodec>(PortCodec.TAG);
        Register<ToCodec>(ToCodec.TAG);
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
                string childTag = child.Name;
                if (!Codecs.ContainsKey(childTag))
                    throw new InvalidOperationException($"Unknown child type '{childTag}'.");
                if (!AllowedChildren.Contains(childTag))
                    throw new InvalidOperationException($"Node '{Tag}' cannot have a childrenOfType child of type '{child.Name}'.");
                if (!Children.ContainsKey(childTag))
                    Children.Add(childTag, new());
                Children[childTag].Add(Instantiate(Codecs[childTag], child));
            }
        }

        foreach (XmlAttribute attribute in xml.Attributes)
        {
            if (!AllowedAttributes.Contains(attribute.Name))
                throw new InvalidOperationException($"Node '{Tag}' cannot have an attribute of type '{attribute.Name}'.");
            Attributes.TryAdd(attribute.Name, attribute.Value);
        }
    }

    /* Public methods. */
    /// <summary>
    /// Convert this node to XML.
    /// </summary>
    public string Serialize()
    {
        // Handle attributes.
        StringBuilder attributes = new();
        foreach (var attr in Attributes)
        {
            if (!AllowedAttributes.Contains(attr.Key))
                throw new KeyNotFoundException($"{GetType().Name} does not allow attribute {attr.Key}.");

            attributes.Append(' ');
            attributes.Append(attr.Key);
            attributes.Append("=\"");
            attributes.Append(attr.Value);
            attributes.Append('"');
        }

        // Handle children.
        StringBuilder children = new();
        foreach (var childrenOfType in Children)
        {
            if (!AllowedChildren.Contains(childrenOfType.Key))
                throw new KeyNotFoundException($"{GetType().Name} does not allow attribute elements with xml {childrenOfType.Key}.");

            foreach (Codec child in childrenOfType.Value)
            {
                if (children.Length > 0)
                    children.Append('\n');
                children.Append(child.Serialize());
            }
        }

        // Build XML.
        StringBuilder xml = new();
        xml.Append('<');
        xml.Append(Tag);
        xml.Append(attributes.ToString());

        if (children.Length > 0)
        {
            xml.Append(">");

            xml.Append("\n\t");
            xml.Append(children.ToString().Replace("\n", "\n\t"));

            xml.Append("\n</");
            xml.Append(Tag);
            xml.Append(">");
        }
        else if (InnerText.Length > 0)
        {
            xml.Append(">");

            xml.Append(InnerText);

            xml.Append("</");
            xml.Append(Tag);
            xml.Append(">");
        }
        else if (children.Length == 0 && InnerText.Length == 0)
            xml.Append("/>");

        return xml.ToString();
    }

    /// <summary>
    /// Compute the checksum of this node and its child nodes.
    /// </summary>
    public virtual void Hash(HashAlgorithm hash)
    {
        // Hash start tag.
        Hash(hash, "<");
        Hash(hash, Tag);

        foreach (var attribute in Attributes)
        {
            Hash(hash, " ");
            Hash(hash, attribute.Key);
            Hash(hash, "=\"");
            Hash(hash, attribute.Value);
            Hash(hash, "\"");
        }

        Hash(hash, ">");

        // Hash contents.
        if (Children.Count == 0)
            Hash(hash, InnerText);
        else
        {
            foreach (var childrenOfType in Children)
            {
                foreach (Codec child in childrenOfType.Value)
                {
                    child.Hash(hash);
                }
            }
        }

        // Hash end tag.
        Hash(hash, "</");
        Hash(hash,  Tag);
        Hash(hash, ">");
    }

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
    /// Get the first child node with some tag. Returns null if the child does not exist.
    /// </summary>
    public Codec GetFirstChild(string tag)
    {
        if (Children.TryGetValue(tag, out var childrenOfType))
        {
            if (childrenOfType.Count > 0)
                return childrenOfType[0];
        }
        return null;
    }

    /// <summary>
    /// Get the first child node with some tag. Returns null if the child does not exist.
    /// </summary>
    public T GetFirstChild<T>()
        where T : Codec
    {
        string tag = Tags[typeof(T)];
        if (GetFirstChild(tag) is T typed)
            return typed;
        else
            throw new InvalidCastException($"Cannot convert codec with tag '{tag}' to type '{typeof(T)}'.");
    }

    public List<Codec> GetChildren(string tag)
    {
        if (Children.TryGetValue(tag, out var childrenOfType))
            return childrenOfType;
        return [];
    }

    public List<T> GetChildren<T>()
        where T : Codec
    {
        string tag = Tags[typeof(T)];
        List<Codec> list = GetChildren(tag);

        List<T> typed = new();
        foreach (Codec codec in list)
        {
            typed.Add(codec as T);
        }
        return typed;
    }

    /// <summary>
    /// Add a node of some type.
    /// </summary>
    public void AddChild(Codec node)
    {
        if (!Children.ContainsKey(node.Tag))
            Children.Add(node.Tag, new());
        Children[node.Tag].Add(node);
    }

    /* Protected methods. */
    /// <summary>
    /// Compute the checksum of a string.
    /// </summary>
    protected static void Hash(HashAlgorithm hash, string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        hash.TransformBlock(bytes, 0, bytes.Length, null, 0);
    }

    /// <summary>
    /// Instantiate a codec node from a type and XML node.
    /// </summary>
    protected static Codec Instantiate(Type type, XmlNode xml)
    {
        if (!type.IsAssignableTo(typeof(Codec)))
            throw new InvalidCastException($"Type {type.Name} is not a codec attribute.");

        ConstructorInfo ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, [typeof(XmlNode)]);
        if (ctor == null)
            throw new NullReferenceException($"Type {type.Name} has no constructor that takes an {nameof(XmlNode)} argument.");

        return ctor.Invoke([xml]) as Codec;
    }

    /* Private methods. */
    private static void Register<T>(string tag)
    {
        Codecs.Add(tag, typeof(T));
        Tags.Add(typeof(T), tag);
    }
}
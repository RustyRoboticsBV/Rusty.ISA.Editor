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

    /* Constructors. */
    static Codec()
    {
        FileCodec.Register();

        // Metadata.
        MetaCodec.Register();
        DataCodec.Register();
        CheckCodec.Register();

        // Schema.
        SchemaCodec.Register();

        InstrsCodec.Register();

        IdefCodec.Register();
        PdefCodec.Register();
        ExecCodec.Register();

        NodesCodec.Register();

        NdefCodec.Register();

        FdefCodec.Register();
        OdefCodec.Register();
        CdefCodec.Register();
        TdefCodec.Register();
        LdefCodec.Register();

        VadefCodec.Register();
        OadefCodec.Register();

        // Graph.
        GraphCodec.Register();

        ElemsCodec.Register();

        NodeCodec.Register();
        JointCodec.Register();
        FrameCodec.Register();
        MemoCodec.Register();

        XCodec.Register();
        YCodec.Register();
        WidthCodec.Register();
        HeightCodec.Register();
        MemberCodec.Register();
        StartCodec.Register();
        TextCodec.Register();
        ColorCodec.Register();

        FormCodec.Register();
        OptionCodec.Register();
        ChoiceCodec.Register();
        TupleCodec.Register();
        ListCodec.Register();
        ArgCodec.Register();

        EdgesCodec.Register();

        EdgeCodec.Register();

        FromCodec.Register();
        PortCodec.Register();
        ToCodec.Register();
    }

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
    public virtual string Serialize()
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
    public virtual void Hash(HashAlgorithm hash) => Hash(hash, Serialize());

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
}
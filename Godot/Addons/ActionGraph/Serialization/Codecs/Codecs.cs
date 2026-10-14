using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A codec factory utility.
/// </summary>
internal static class Codecs
{
    /* Constants. */
    public const string ID = "id";
    public const string Editor = "editor";
    public const string Checksum = "csum";
    public const string Value = "value";
    public const string ExecutionHandler = "exec";
    public const string Localizable = "loc";
    public const string Definition = "type";
    public const string X = "x";
    public const string Y = "y";
    public const string Width = "width";
    public const string Height = "height";
    public const string Folded = "folded";
    public const string ParentFrame = "member";
    public const string Start = "start";
    public const string Text = "text";
    public const string Color = "color";
    public const string Index = "index";
    public const string NoDefault = "nodflt";
    public const string Edge = "edge";
    public const string From = "from";
    public const string Port = "port";
    public const string To = "to";

    /* Public properties. */
    public static int Count
    {
        get
        {
            Initialize();
            return Names.Count;
        }
    }

    /* Private properties. */
    private static List<string> Names = null;
    private static Dictionary<string, int> Indices = null;
    private static Dictionary<Type, string> TypeTags = null;
    private static List<ConstructorInfo> Ctors = null;

    /* Public methods. */
    public static int GetIndex(string name)
    {
        Initialize();
        return Indices[name];
    }
    public static int GetIndex(Codec codec) => GetIndex(codec.GetType());
    public static int GetIndex<T>() => GetIndex(GetTag<T>());
    public static int GetIndex(Type type) => GetIndex(GetTag(type));

    public static string GetName(int index)
    {
        Initialize();
        return Names[index];
    }
    public static string GetTag<T>()
    {
        Initialize();
        return TypeTags[typeof(T)];
    }
    public static string GetTag(Type type)
    {
        Initialize();
        return TypeTags[type];
    }

    /// <summary>
    /// Get the documented type of an attribute.
    /// </summary>
    public static AttributeType GetAttributeType(string attribute)
    {
        return attribute switch
        {
            Localizable or
            NoDefault or
            Folded => AttributeType.Bool,

            X or
            Y => AttributeType.Int,

            Width or
            Height or
            Index or
            Port => AttributeType.Uint,

            Color => AttributeType.Color,

            _ => AttributeType.String
        };
    }

    /// <summary>
    /// Instantiate a codec node.
    /// </summary>
    public static Codec Instantiate(string name)
    {
        Initialize();

        if (!Indices.TryGetValue(name, out int index))
            throw new InvalidOperationException($"Unknown codec '{name}'.");

        return Instantiate(index);
    }
    /// <summary>
    /// Instantiate a codec node.
    /// </summary>
    public static Codec Instantiate(int index)
    {
        Initialize();
        return (Codec)Ctors[index].Invoke([]);
    }

    /* Private methods. */
    private static void Initialize()
    {
        if (Names != null && Indices != null && TypeTags != null && Ctors != null)
            return;

        Names = new();
        Indices = new();
        TypeTags = new();
        Ctors = new();

        Register<FileCodec>(FileCodec.TAG);

        Register<MetaCodec>(MetaCodec.TAG);
        Register<LangCodec>(LangCodec.TAG);

        Register<IdefCodec>(IdefCodec.TAG);
        Register<PdefCodec>(PdefCodec.TAG);

        Register<NdefCodec>(NdefCodec.TAG);

        Register<FdefCodec>(FdefCodec.TAG);
        Register<OdefCodec>(OdefCodec.TAG);
        Register<CdefCodec>(CdefCodec.TAG);
        Register<TdefCodec>(TdefCodec.TAG);
        Register<LdefCodec>(LdefCodec.TAG);

        Register<VdefCodec>(VdefCodec.TAG);
        Register<JdefCodec>(JdefCodec.TAG);

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
        Register<LocCodec>(LocCodec.TAG);
        Register<OutCodec>(OutCodec.TAG);
    }

    private static void Register<T>(string name)
        where T : Codec
    {
        int index = Names.Count;

        if (Indices.ContainsKey(name))
            throw new InvalidOperationException($"A codec is already registered for tag '{name}'.");

        Names.Add(name);
        Indices.Add(name, index);
        TypeTags.Add(typeof(T), name);
        Ctors.Add(GetCtor(typeof(T), []));
    }

    private static ConstructorInfo GetCtor(Type type, Type[] args)
    {
        ConstructorInfo ctor = type.GetConstructor(
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic,
            binder: null,
            types: args,
            modifiers: null
        );

        if (ctor == null)
            throw new InvalidOperationException($"Codec '{type.Name}' is missing a ctor.");

        return ctor;
    }
}
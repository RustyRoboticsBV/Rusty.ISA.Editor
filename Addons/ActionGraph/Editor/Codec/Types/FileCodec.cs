using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class FileCodec : Codec
{
    /* Constants. */
    public const string TAG = "file";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [MetaCodec.TAG, SchemaCodec.TAG, GraphCodec.TAG];

    /* Constructors. */
    public FileCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    public static void Register() => Codecs.Add(TAG, typeof(FileCodec));
}
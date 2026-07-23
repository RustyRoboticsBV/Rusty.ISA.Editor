using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class CheckCodec : Codec
{
    /* Constants. */
    public const string TAG = "check";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [Value];

    /* Constructors. */
    public CheckCodec() : base() { }

    public CheckCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    public override void Hash(HashAlgorithm hash)
    {
        Hash(hash, "<");
        Hash(hash, Tag);
        Hash(hash, " ");
        Hash(hash, Value);
        Hash(hash, "=\"");
        Hash(hash, GetAttribute(Value));
        Hash(hash, "\"/>");
    }
}
using Rusty.ActionGraph.Serialization;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Documentation;

/// <summary>
/// Generates Markdown documentation for registered codecs.
/// </summary>
internal static class Descriptions
{
    public static Dictionary<string, string> Attributes = new()
    {
        { Codecs.ID, "The identifier of the element." },
        { Codecs.Editor, "The editor that generated the file." },
        { Codecs.Checksum, "The checksum of the file." },
        { Codecs.Definition, "The definition for this [[CODEC_NAME]]." }
    };
}
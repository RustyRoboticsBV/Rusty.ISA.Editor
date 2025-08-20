using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A repository for all preview scripts in the program.
/// </summary>
public static class PreviewDict
{
    /* Private properties. */
    private static Dictionary<InstructionResource, Preview> Previews { get; } = new();

    /* Public methods. */
    /// <summary>
    /// Add a new preview for some resource.
    /// </summary>
    public static void Add(InstructionResource resource, Preview preview)
    {
        Previews.Add(resource, preview);
    }

    /// <summary>
    /// Check if there is a preview for some resource.
    /// </summary>
    public static bool Has(InstructionResource resource)
    {
        return Previews.ContainsKey(resource);
    }

    /// <summary>
    /// Create an instance of a resource's preview and return it.
    /// </summary>
    public static PreviewInstance CreateInstance(InstructionResource resource)
    {
        return Previews[resource].CreateInstance();
    }

    /// <summary>
    /// Delete all previews.
    /// </summary>
    public static void Clear()
    {
        Previews.Clear();
    }
}
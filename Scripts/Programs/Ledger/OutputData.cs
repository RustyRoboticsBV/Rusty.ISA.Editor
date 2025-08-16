using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// Contains output data related to a program unit.
/// </summary>
public class OutputData
{
    /* Public properties. */
    public bool HasDefaultOutput { get; set; } = true;
    public List<string> Labels { get; } = new();

    /* Public methods. */
    public void AddOutput(string label, bool hideDefaultOutput)
    {
        if (hideDefaultOutput)
            HasDefaultOutput = false;
        Labels.Add(label);
    }

    public List<string> GetAllOutputs()
    {
        List<string> labels = new(Labels);
        if (HasDefaultOutput)
            labels.Insert(0, "");
        return labels;
    }
}
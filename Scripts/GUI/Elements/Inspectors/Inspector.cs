using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public partial class Inspector : MarginContainer, IGuiElement
{
    /* Public properties. */
    public IContainer ContentsContainer { get; private set; }

    /* Private properties. */
    private Dictionary<string, string> MetaData { get; } = new();
    private BiDict<string, int> Elements { get; } = new();

    /* Constructors. */
    public Inspector()
    {
        VerticalContainer container = new();
        AddChild(container);
        ContentsContainer = container;
    }

    /* Public methods. */
    public IGuiElement Copy()
    {
        Inspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(IGuiElement other)
    {
        if (other is Inspector inspector)
        {
            // Copy metadata.
            MetaData.Clear();
            foreach (var metaData in inspector.MetaData)
            {
                MetaData.Add(metaData.Key, metaData.Value);
            }

            // Replace contents container with a copy of the other inspector's contents container.
            Clear();

            RemoveChild(ContentsContainer as Node);
            ContentsContainer = inspector.ContentsContainer.Copy() as IContainer;
            AddChild(ContentsContainer as Node);

            foreach (string key in inspector.Elements.LeftValues)
            {
                Elements.Add(key, inspector.Elements[key]);
            }

            TooltipText = inspector.TooltipText;
        }
    }

    public void WriteMetaData(string key, string value)
    {
        if (MetaData.ContainsKey(key))
            MetaData[key] = value;
        else
            MetaData.Add(key, value);
    }

    public string ReadMetaData(string key)
    {
        if (MetaData.ContainsKey(key))
            return MetaData[key];
        else
            return "";
    }

    /// <summary>
    /// Get the number of elements on this inspector.
    /// </summary>
    public int GetContentsCount()
    {
        return Elements.Count;
    }

    /// <summary>
    /// Get the key associated with the index of an element from the contents.
    /// </summary>
    public string GetKey(int index)
    {
        return Elements[index];
    }

    /// <summary>
    /// Get an element on this inspector.
    /// </summary>
    public IGuiElement GetAt(string key)
    {
        int index = Elements[key];
        return ContentsContainer.GetFromContents(index);
    }

    /// <summary>
    /// Get an element on this inspector.
    /// </summary>
    public IGuiElement GetAt(int index)
    {
        return ContentsContainer.GetFromContents(index);
    }

    /// <summary>
    /// Add an element to this inspector.
    /// </summary>
    public void Add(string key, IGuiElement element)
    {
        ContentsContainer.AddToContents(element);
        Elements.Add(key, ContentsContainer.GetContentsCount() - 1);
    }

    /// <summary>
    /// Remove an element from this inspector.
    /// </summary>
    public void Remove(string key)
    {
        IGuiElement element = GetAt(key);
        ContentsContainer.RemoveFromContents(element);
        Elements.Remove(key);
    }

    /// <summary>
    /// Clear all contents.
    /// </summary>
    public void Clear()
    {
        Elements.Clear();
        ContentsContainer.ClearContents();
    }

    /// <summary>
    /// Replace the contents container of this inspector. This preserves all the elements and their keys.
    /// </summary>
    public void ReplaceContainer(IContainer container)
    {
        if (container == null)
            return;

        // Make sure that the new container is empty.
        container.ClearContents();

        // Transfer contents to the new container.
        while (ContentsContainer.GetContentsCount() > 0)
        {
            IGuiElement element = ContentsContainer.GetFromContents(0);
            ContentsContainer.RemoveFromContents(element);
            container.AddToContents(element);
        }

        // Replace contents container.
        RemoveChild(ContentsContainer as Node);
        ContentsContainer = container;
        AddChild(container as Node);
    }
}
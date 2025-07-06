using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A vertical container for GUI elements.
/// </summary>
public partial class VerticalContainer : VBoxContainer, IGuiElement
{
    /* Private properties. */
    private List<IGuiElement> Elements { get; } = new();

    /* Public methods. */
    public IGuiElement Copy()
    {
        VerticalContainer copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(IGuiElement other)
    {
        Clear();
        if (other is VerticalContainer hbox)
        {
            for (int i = 0; i < hbox.Elements.Count; i++)
            {
                Elements.Add(hbox.Elements[i].Copy());
            }
        }
    }

    public int GetCount()
    {
        return Elements.Count;
    }

    public IGuiElement GetAt(int index)
    {
        return GetChild(index) as IGuiElement;
    }

    public void Add(IGuiElement element)
    {
        AddChild(element as Node);
    }

    public void Remove(IGuiElement element)
    {
        RemoveChild(element as Node);
    }

    public void Clear()
    {
        while (Elements.Count > 0)
        {
            RemoveChild(Elements[0] as Node);
            Elements.RemoveAt(0);
        }
    }
}
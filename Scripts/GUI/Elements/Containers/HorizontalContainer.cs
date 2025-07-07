using Godot;
using System.Collections.Generic;
using System.Reflection;

namespace Rusty.ISA.Editor;

/// <summary>
/// A horizontal container for GUI elements.
/// </summary>
public partial class HorizontalContainer : HBoxContainer, IContainer
{
    /* Private properties. */
    private List<IGuiElement> Elements { get; } = new();

    /* Public methods. */
    public IGuiElement Copy()
    {
        HorizontalContainer copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(IGuiElement other)
    {
        if (other is HorizontalContainer hbox)
        {
            ClearContents();
            for (int i = 0; i < hbox.Elements.Count; i++)
            {
                AddToContents(hbox.Elements[i].Copy());
            }

            TooltipText = hbox.TooltipText;
        }
    }

    public int GetContentsCount()
    {
        return Elements.Count;
    }

    public IGuiElement GetFromContents(int index)
    {
        return Elements[index];
    }

    public void AddToContents(IGuiElement element)
    {
        Elements.Add(element);
        AddChild(element as Node);
    }

    public void RemoveFromContents(IGuiElement element)
    {
        Elements.Remove(element);
        RemoveChild(element as Node);
    }

    public void ClearContents()
    {
        foreach (IGuiElement element in Elements)
        {
            RemoveChild(element as Node);
        }
        Elements.Clear();
    }
}
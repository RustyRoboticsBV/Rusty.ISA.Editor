using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A vertical container for GUI elements.
/// </summary>
public partial class VerticalContainer : VBoxContainer, IContainer
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
        if (other is VerticalContainer vbox)
        {
            GD.Print("VBOX: Begin copy from " + vbox);
            ClearContents();
            for (int i = 0; i < vbox.Elements.Count; i++)
            {
                GD.Print("VBOX: Copying " + vbox.Elements[i]);
                AddToContents(vbox.Elements[i].Copy());
            }
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
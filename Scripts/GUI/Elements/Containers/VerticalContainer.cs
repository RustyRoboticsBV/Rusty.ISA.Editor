using Godot;
using System.Collections.Generic;
using System.Reflection;

namespace Rusty.ISA.Editor;

/// <summary>
/// A vertical container for GUI elements.
/// </summary>
public partial class VerticalContainer : VBoxContainer, IContainer
{
    /* Private properties. */
    private List<IGuiElement> Elements { get; } = new();

    /* Public events. */
    public event ChangedHandler Changed;

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
            ClearContents();
            for (int i = 0; i < vbox.Elements.Count; i++)
            {
                AddToContents(vbox.Elements[i].Copy());
            }

            TooltipText = vbox.TooltipText;
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
        element.Changed += OnElementChanged;
        Changed?.Invoke();
    }

    public void RemoveFromContents(IGuiElement element)
    {
        Elements.Remove(element);
        RemoveChild(element as Node);
        Changed?.Invoke();
    }

    public void ClearContents()
    {
        foreach (IGuiElement element in Elements)
        {
            RemoveChild(element as Node);
        }
        Elements.Clear();
        Changed?.Invoke();
    }

    /* Private methods. */
    private void OnElementChanged()
    {
        Changed?.Invoke();
    }
}
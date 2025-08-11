using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A horizontal container for GUI elements.
/// </summary>
public partial class HorizontalContainer : HBoxContainer, IContainer
{
    /* Private properties. */
    private List<IGuiElement> Elements { get; } = new();

    /* Public events. */
    public event ChangedHandler Changed;

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
        Changed?.Invoke();
        element.Changed += OnElementChanged;
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
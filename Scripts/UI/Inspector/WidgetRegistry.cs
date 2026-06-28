using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ISA;

/// <summary>
/// A numeric field.
/// </summary>
public static class WidgetRegistry
{
    /* Private properties. */
    private static List<IWidget> Widgets { get; } = new();

    /* Public methods. */
    public static void Add(IWidget widget)
    {
        Widgets.Add(widget);
    }

    public static void Remove(IWidget widget)
    {
        Widgets.Remove(widget);
    }

    public static void ReleaseFocus()
    {
        foreach (IWidget widget in Widgets)
        {
            widget.CancelFocus();
        }
    }
}
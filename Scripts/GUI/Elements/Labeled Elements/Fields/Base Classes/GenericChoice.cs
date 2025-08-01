﻿using Godot;
using System;

namespace Rusty.ISA.Editor;

/// <summary>
/// A base class for choice elements.
/// </summary>
public abstract partial class GenericChoice<T> : LabeledElement, IChoiceField
    where T : Control, new()
{
    /* Public properties. */
    object IField.Value
    {
        get => Selected;
        set => Selected = (int)value;
    }
    public override string TooltipText
    {
        get => base.TooltipText;
        set
        {
            base.TooltipText = value;
            Field.TooltipText = value;
        }
    }
    public abstract string[] Options { get; set; }
    public abstract int Selected { get; set; }

    /* Protected properties. */
    protected T Field { get; set; }

    /* Constructors. */
    public GenericChoice()
    {
        // Add field control.
        Field = new();
        Field.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Field);
    }

    /* Public methods. */
    public override void CopyFrom(IGuiElement other)
    {
        base.CopyFrom(other);

        if (other is GenericChoice<T> field)
        {
            string[] options = field.Options;
            Options = new string[options.Length];
            Array.Copy(options, Options, options.Length);
            Selected = field.Selected;
        }
    }
}
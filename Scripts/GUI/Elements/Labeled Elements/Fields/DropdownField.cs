using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A dropdown field.
/// </summary>
public partial class DropdownField : GenericChoice<OptionButton>
{
    /* Public properties. */
    public override string[] Options
    {
        get
        {
            List<string> options = new();
            for (int i = 0; i < Field.ItemCount; i++)
            {
                options.Add(Field.GetItemText(i));
            }
            return options.ToArray();
        }
        set
        {
            Field.Clear();
            foreach (string option in value)
            {
                Field.AddItem(option);
            }
        }
    }
    public override int Selected
    {
        get => Field.Selected;
        set => Field.Selected = value;
    }
    public string SelectedText => Field.GetItemText(Field.Selected);

    /* Constructors. */
    public DropdownField()
    {
        Field.CustomMinimumSize = new(0f, 32f);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        DropdownField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    /// <summary>
    /// Set the dropdown options using all the values from an enum.
    /// </summary>
    public void SetOptionsFromEnum<T>() where T : struct, Enum
    {
        T[] values = Enum.GetValues<T>();
        List<string> options = new();
        foreach (object option in values)
        {
            options.Add(option.ToString());
        }
        Options = options.ToArray();
    }
}
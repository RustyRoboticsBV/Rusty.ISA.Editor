using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A dropdown field.
/// </summary>
public partial class DropdownField : LabeledElement, Field
{
    /* Public properties. */
    object Field.Value
    {
        get => Selected;
        set => Selected = (int)value;
    }
    public int Selected
    {
        get => Field.Selected;
        set => Field.Selected = value;
    }
    public string SelectedOption => Field.GetItemText(Field.Selected);
    public string[] Options
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

    /* Private properties. */
    private OptionButton Field { get; set; }

    /* Public methods. */
    public override DropdownField Copy()
    {
        DropdownField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(DropdownField other)
    {
        CopyFrom(other as LabeledElement);
        Options = other.Options;
        Selected = other.Selected;
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

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add option button.
        Field = new();
        Field.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Field.CustomMinimumSize = new(0f, 32f);
        AddChild(Field);
    }
}
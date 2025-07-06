using Godot;
using System;

namespace Rusty.ISA.Editor;

/// <summary>
/// An enum dropdown field.
/// </summary>
public partial class EnumField : LabeledElement, Field
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
    public Enum Enum
    {
        get
        {
            return _Enum;
        }
        set
        {
            _Enum = value;
            Field.Clear();
            foreach (var option in Enum.GetValues(value.GetType()))
            {
                Field.AddItem(option.ToString());
            }
        }
    }

    /* Private properties. */
    private OptionButton Field { get; set; }
    private Enum _Enum { get; set; }

    /* Public methods. */
    public override EnumField Copy()
    {
        EnumField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(EnumField other)
    {
        CopyFrom(other as LabeledElement);
        Enum = other.Enum;
        Selected = other.Selected;
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
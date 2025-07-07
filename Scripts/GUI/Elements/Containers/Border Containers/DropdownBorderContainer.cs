using System;

namespace Rusty.ISA.Editor;

public partial class DropdownBorderContainer : BorderContainer
{
    /* Public properties. */
    public string DropdownText
    {
        get => Dropdown.LabelText;
        set => Dropdown.LabelText = value;
    }
    public string[] Options
    {
        get => Dropdown.Options;
        set => Dropdown.Options = value;
    }
    public int SelectedOption
    {
        get => Dropdown.Selected;
        set => Dropdown.Selected = value;
    }

    /* Private properties. */
    private DropdownField Dropdown => GetFromHeader(0) as DropdownField;

    /* Constructors. */
    public DropdownBorderContainer() : base()
    {
        // Add dropdown.
        DropdownField dropdown = new();
        dropdown.LabelText = "";
        dropdown.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddToHeader(dropdown);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        DropdownBorderContainer copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    /* Godot overrides. */
    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public override void _Ready()
    {
        _Process(0.0);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        // Hide or show contents based on the selected option.
        for (int i = 0; i < GetContentsCount(); i++)
        {
            GetFromContents(i).Visible = Dropdown.Selected == i;
        }
    }
}
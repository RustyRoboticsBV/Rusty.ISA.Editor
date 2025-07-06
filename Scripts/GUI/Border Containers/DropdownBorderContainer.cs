using Godot;
using System.Collections.Generic;

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
    private DropdownField Dropdown { get; set; }
    private List<VBoxContainer> OptionContents { get; set; } = new();

    /* Public methods. */
    public void AddToOption(Control control, int tab)
    {
        while (OptionContents.Count < Options.Length)
        {
            VBoxContainer contents = new();
            AddToContents(contents);
            OptionContents.Add(contents);
        }

        OptionContents[tab].AddChild(control);
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

        for (int i = 0; i < OptionContents.Count; i++)
        {
            OptionContents[i].Visible = Dropdown.Selected == i;
        }
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add dropdown.
        Dropdown = new();
        Dropdown.LabelText = "";
        Dropdown.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddToHeader(Dropdown);
    }
}
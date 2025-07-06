using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public partial class TabBorderContainer : BorderContainer
{
    /* Public properties. */
    public string[] Tabs
    {
        get => TabBar.Tabs;
        set => TabBar.Tabs = value;
    }

    /* Private properties. */
    private TabBarField TabBar { get; set; }
    private List<VBoxContainer> TabContents { get; set; } = new();

    /* Public methods. */
    public void AddToTab(Control control, int tab)
    {
        while (TabContents.Count < Tabs.Length)
        {
            VBoxContainer contents = new();
            AddToContents(contents);
            TabContents.Add(contents);
        }

        TabContents[tab].AddChild(control);
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

        for (int i = 0; i < TabContents.Count; i++)
        {
            TabContents[i].Visible = TabBar.Selected == i;
        }
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add tab-bar.
        //HBoxContainer container = new();
        //container.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        //AddToHeader(container);
        TabBar = new();
        TabBar.LabelText = "";
        TabBar.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddToHeader(TabBar);
    }
}
using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public partial class TabBorderContainer : BorderContainer
{
    /* Public properties. */
    public string[] Tabs
    {
        get => TabBar.Options;
        set => TabBar.Options = value;
    }
    public int SelectedTab
    {
        get => TabBar.Selected;
        set => TabBar.Selected = value;
    }

    /* Private properties. */
    private TabBarField TabBar => GetFromHeader(0) as TabBarField;

    /* Public methods. */
    public override IGuiElement Copy()
    {
        TabBorderContainer copy = new();
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

        for (int i = 0; i < GetContentsCount(); i++)
        {
            GetFromContents(i).Visible = TabBar.Selected == i;
        }
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add tab-bar.
        TabBarField tabBar = new();
        tabBar.LabelText = "";
        tabBar.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddToHeader(tabBar);
    }
}
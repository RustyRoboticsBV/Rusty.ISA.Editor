using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A tab-bar field.
/// </summary>
public partial class TabBarField : GenericChoice<TabBar>
{
    /* Public properties. */
    public override string[] Options
    {
        get
        {
            List<string> tabs = new();
            for (int i = 0; i < Field.TabCount; i++)
            {
                tabs.Add(Field.GetTabTitle(i));
            }
            return tabs.ToArray();
        }
        set
        {
            Field.ClearTabs();
            foreach (string tab in value)
            {
                Field.AddTab(tab);
            }
            InvokeChanged();
        }
    }
    public override int Selected
    {
        get => Field.CurrentTab;
        set
        {
            Field.CurrentTab = value;
            InvokeChanged();
        }
    }
    public string SelectedText => Field.GetTabTitle(Field.CurrentTab);

    /* Constructors. */
    public TabBarField()
    {
        Field.CustomMinimumSize = new(0f, 32f);
        Field.TabButtonPressed += OnTabPressed;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        TabBarField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        base._Process(delta);

        Field.CustomMinimumSize = new(CalculateDesiredWidth(Field), Field.CustomMinimumSize.Y);
    }

    /* Private methods. */
    private static float CalculateDesiredWidth(TabBar tabBar)
    {
        float totalWidth = 0f;
        int tabCount = tabBar.GetTabCount();

        for (int i = 0; i < tabCount; i++)
        {
            Rect2 tabRect = tabBar.GetTabRect(i);
            totalWidth += tabRect.Size.X;
        }

        return totalWidth;
    }

    private void OnTabPressed(long tab)
    {
        InvokeChanged();
    }
}
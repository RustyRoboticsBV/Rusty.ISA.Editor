using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A color field.
/// </summary>
public partial class TabBarField : LabeledElement, Field
{
    /* Public properties. */
    object Field.Value
    {
        get => Selected;
        set => Selected = (int)value;
    }
    public int Selected
    {
        get => Field.CurrentTab;
        set => Field.CurrentTab = value;
    }
    public string[] Tabs
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
        }
    }
    public string SelectedTab => Field.GetTabTitle(Field.CurrentTab);

    /* Private properties. */
    private TabBar Field { get; set; }

    /* Public methods. */
    public override TabBarField Copy()
    {
        TabBarField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(TabBarField other)
    {
        CopyFrom(other as LabeledElement);
        Tabs = other.Tabs;
        Selected = other.Selected;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        base._Process(delta);

        Field.CustomMinimumSize = new(CalculateDesiredWidth(Field), Field.CustomMinimumSize.Y);
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add color picker button.
        Field = new();
        Field.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Field.CustomMinimumSize = new(0f, 32f);
        AddChild(Field);
    }

    /* Private methods. */
    private static float CalculateDesiredWidth(TabBar tabBar)
    {
        /*float totalWidth = 0f;

        // Get theme-related values
        Font font = tabBar.GetThemeFont("font");
        int fontSize = tabBar.GetThemeFontSize("font_size");
        int hSeparation = tabBar.GetThemeConstant("hseparation");
        int tabMargin = tabBar.GetThemeConstant("tab_margin") * 2; // left + right

        for (int i = 0; i < tabBar.GetTabCount(); i++)
        {
            string title = tabBar.GetTabTitle(i);
            Vector2 textSize = font.GetStringSize(title, HorizontalAlignment.Left, fontSize);
            totalWidth += textSize.X + tabMargin + hSeparation;
        }

        // Remove the final hSeparation (not needed after last tab)
        totalWidth -= hSeparation;  

        return totalWidth;*/

        /*float totalWidth = 0f;

        // Grab theme resources
        Font font = tabBar.GetThemeFont("font");
        int fontSize = tabBar.GetThemeFontSize("font_size");

        // Tab stylebox controls padding
        StyleBox stylebox = tabBar.GetThemeStylebox("tab");

        // Other tab spacing and padding
        int tabMargin = tabBar.GetThemeConstant("tab_margin");  // padding inside each tab
        int hSeparation = tabBar.GetThemeConstant("hseparation"); // space between tabs

        for (int i = 0; i < tabBar.GetTabCount(); i++)
        {
            string title = tabBar.GetTabTitle(i);

            // Use MeasureText to get more realistic width
            //float textWidth = font.MeasureText(title, fontSize).X;
            float textWidth = font.GetStringSize(title, HorizontalAlignment.Left, fontSize).X;

            // Add icon width if the tab has one
            Texture2D icon = tabBar.GetTabIcon(i);
            float iconWidth = 0f;
            if (icon != null)
                iconWidth = icon.GetWidth() + 4; // +4 for spacing between icon and text

            // Total width per tab = text + icon + margins + stylebox padding
            float tabWidth = textWidth + iconWidth
                             + 2 * tabMargin
                             + stylebox.GetMargin(Side.Left)
                             + stylebox.GetMargin(Side.Right);

            totalWidth += tabWidth + hSeparation;
        }

        // Remove last hSeparation
        if (tabBar.GetTabCount() > 0)
            totalWidth -= hSeparation;

        return totalWidth;*/

        float totalWidth = 0f;
        int tabCount = tabBar.GetTabCount();

        for (int i = 0; i < tabCount; i++)
        {
            // This returns the actual drawn rect for the tab (position + size)
            Rect2 tabRect = tabBar.GetTabRect(i);
            totalWidth += tabRect.Size.X;
        }

        /*GD.Print(totalWidth);*/
        return totalWidth;
    }
}
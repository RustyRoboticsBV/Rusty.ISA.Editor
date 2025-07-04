using Godot;
using System;

namespace Rusty.ISA.Editor;

/// <summary>
/// An IS Aeditor graph comment.
/// </summary>
public partial class GraphComment : Godot.GraphNode, IGraphElement
{
    /* Public properties. */
    public GraphEdit GraphEdit
    {
        get
        {
            Node parent = GetParent();
            if (parent is GraphEdit graphEdit)
                return graphEdit;
            else
                return null;
        }
    }
    public GraphFrame Frame { get; set; }

    public string Text { get; set; } = "Default text.";
    public Color BgColor { get; set; } = new Color(0.13f, 0.13f, 0.13f, 0.75f);
    public Color TextColor { get; set; } = new Color(0f, 1f, 0f, 1f);

    /* Public events. */
    public new event Action<IGraphElement> NodeSelected;
    public new event Action<IGraphElement> NodeDeselected;
    public new event Action<IGraphElement> Dragged;
    public new event Action<IGraphElement> DeleteRequest;

    /* Private properties. */
    private RichTextLabel Label { get; set; }

    /* Public methods. */
    public bool IsNestedIn(GraphFrame frame)
    {
        return Frame == frame || Frame != null && Frame.IsNestedIn(frame);
    }

    public void RequestDelete()
    {
        DeleteRequest?.Invoke(this);
    }

    /* Constructors. */
    public GraphComment()
    {
        // Add contents.
        Label = new()
        {
            Name = "Text",
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill,
            FitContent = true,
            AutowrapMode = TextServer.AutowrapMode.Off,
            MouseFilter = MouseFilterEnum.Pass
        };

        HBoxContainer contents = new()
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        contents.AddChild(Label);

        MarginContainer margin = new();
        margin.AddThemeConstantOverride("margin_top", 8);
        margin.AddThemeConstantOverride("margin_left", 16);
        margin.AddChild(contents);
        AddChild(margin);

        // Add style overrides.
        AddThemeStyleboxOverride("panel_selected", new StyleBoxFlat()
        {
            BgColor = EditorNodeInfo.SelectedMainColor
        });
        AddThemeStyleboxOverride("titlebar_selected", new StyleBoxFlat()
        {
            BgColor = EditorNodeInfo.SelectedMainColor
        });

        // Empty the title container.
        if (GetChild(0, true) is Control titleContainer)
        {
            while (titleContainer.GetChildCount(true) > 0)
            {
                titleContainer.RemoveChild(titleContainer.GetChild(0, true));
            }
        }
    }

    public override void _Process(double delta)
    {
        // Update background color.
        AddThemeStyleboxOverride("panel", new StyleBoxFlat()
        {
            BgColor = BgColor
        });
        AddThemeStyleboxOverride("titlebar", new StyleBoxFlat()
        {
            BgColor = BgColor
        });

        // Update label text color.
        if (Label != null)
        {
            Label.Size = Vector2.Zero;
            Label.Text = Text + " ";
            Label.AddThemeColorOverride("default_color", Selected ? EditorNodeInfo.SelectedTextColor : TextColor);
        }

        // Update size.
        Size = Vector2.Zero;
        Size += new Vector2(10f, 10f);
    }
}
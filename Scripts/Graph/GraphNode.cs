using Godot;
using System;
using System.Xml.Linq;

namespace Rusty.ISA.Editor;

public partial class GraphNode : Godot.GraphNode, IGraphElement
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

    /* Private properties. */
    private Label TitleLabel { get; set; }
    private TextureRect TitleIcon { get; set; }

    /* Public events. */
    public new event Action<IGraphElement> NodeSelected;
    public new event Action<IGraphElement> NodeDeselected;
    public new event Action<IGraphElement> Dragged;
    public new event Action<IGraphElement> DeleteRequest;

    /* Constructors. */
    public GraphNode()
    {
        // Fix title.
        HBoxContainer titleContainer = GetChild(0, true) as HBoxContainer;
        titleContainer.RemoveChild(titleContainer.GetChild(0, true));
        TitleIcon = new();
        titleContainer.AddChild(TitleIcon);
        TitleLabel = new();
        titleContainer.AddChild(TitleLabel);

        // Set default values.
        AddThemeColorOverride("font_color", Colors.White);
        SetTitleColor(new Color(0.5f, 0.5f, 0.5f));
        SetPanelColor(new Color(0.13f, 0.13f, 0.13f));
        SetTitle("Node");

        // Subscribe to events.
        base.NodeSelected += OnNodeSelected;
        base.NodeDeselected += OnNodeDeselected;
        base.Dragged += OnDragged;
        base.DeleteRequest += OnDeleteRequest;
    }

    /* Public methods. */
    public bool IsNestedIn(GraphFrame frame)
    {
        return Frame == frame || Frame != null && Frame.IsNestedIn(frame);
    }

    public void RequestDelete()
    {
        DeleteRequest?.Invoke(this);
    }

    public void SetTitleColor(Color color)
    {
        AddThemeStyleboxOverride("titlebar", new StyleBoxFlat() { BgColor = color });
        AddThemeStyleboxOverride("titlebar_selected", new StyleBoxFlat() { BgColor = Colors.White });
    }

    public void SetPanelColor(Color color)
    {
        AddThemeStyleboxOverride("panel", new StyleBoxFlat() { BgColor = color });
        AddThemeStyleboxOverride("panel_selected", new StyleBoxFlat() { BgColor = color });
    }

    public void SetIcon(Texture2D icon)
    {
        TitleIcon.Texture = icon;
    }

    public new void SetTitle(string title)
    {
        TitleLabel.Text = title;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Selected)
        {
            TitleIcon.Modulate = Colors.Gray;
            TitleLabel.Modulate = Colors.Gray;
        }
        else
        {
            TitleIcon.Modulate = Colors.White;
            TitleLabel.Modulate = Colors.White;
        }
    }

    /* Private methods. */
    private void OnNodeSelected()
    {
        NodeSelected?.Invoke(this);
    }

    private void OnNodeDeselected()
    {
        NodeDeselected?.Invoke(this);
    }

    private void OnDeleteRequest()
    {
        RequestDelete();
    }

    private void OnDragged(Vector2 from, Vector2 to)
    {
        Dragged?.Invoke(this);
    }
}
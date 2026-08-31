using Godot;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Editor;

public sealed partial class GraphNode : GraphElement
{
    /* Public properties. */
    public string HeaderText
    {
        get => TitleLabel.Text;
        set => TitleLabel.Text = value;
    }

    public Texture2D HeaderIcon
    {
        get => Icon.Texture;
        set => Icon.Texture = value;
    }

    public string PreviewText
    {
        get => Preview.Text;
        set
        {
            Preview.Text = value;
            Preview.Visible = !string.IsNullOrEmpty(value);
        }
    }

    /* Private properties. */
    private Label TitleLabel { get; set; }
    private TextureRect Icon { get; set; }
    private VBoxContainer Contents { get; set; }
    private VBoxContainer OutputContainer { get; set; }
    private Label Preview { get; set; }

    private InputPortContainer Input { get; } = new();
    private List<OutputPortContainer> Outputs { get; } = new();

    /* Constructors. */
    public GraphNode()
    {
        PanelContainer panel = new();
        panel.Name = "Panel";
        panel.MouseFilter = MouseFilterEnum.Ignore;
        PanelUtility.SetPanelStyle(panel, PanelUtility.GetStyleBox(Color.FromHtml("1A1A1A"), 4, 4, 4, 4));
        AddChild(panel);

        Contents = new();
        Contents.Name = "Contents";
        Contents.MouseFilter = MouseFilterEnum.Ignore;
        panel.AddChild(Contents);

        // Create header.
        PanelContainer header = new();
        header.Name = "Title Panel";
        header.MouseFilter = MouseFilterEnum.Ignore;
        header.CustomMinimumSize = new(0f, 32f);
        PanelUtility.SetPanelStyle(header, PanelUtility.GetStyleBox(Color.FromHtml("0088FF"), 4, 4, 0, 0));
        Contents.AddChild(header);

        MarginContainer headerMargin = MarginUtility.Create(0, 0, 16, 0);
        header.AddChild(headerMargin);

        HBoxContainer headerHbox = new();
        headerMargin.AddChild(headerHbox);

        MarginContainer iconMargin = MarginUtility.Create(4);
        headerHbox.AddChild(iconMargin);

        Icon = new();
        Icon.ExpandMode = TextureRect.ExpandModeEnum.FitWidth;
        Icon.CustomMinimumSize = new(22, 22);
        iconMargin.AddChild(Icon);

        TitleLabel = new();
        TitleLabel.Text = "New Node";
        headerHbox.AddChild(TitleLabel);

        // Create ports.
        HBoxContainer portContainer = new();
        portContainer.Name = "Ports";
        Contents.AddChild(portContainer);

        VBoxContainer inputContainer = new();
        inputContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        inputContainer.Name = "Inputs";
        portContainer.AddChild(inputContainer);

        Control separator = new();
        separator.Name = "Separator";
        portContainer.AddChild(separator);

        OutputContainer = new();
        OutputContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        OutputContainer.Name = "Outputs";
        portContainer.AddChild(OutputContainer);

        InputPortContainer input = new();
        input.Name = "Input";
        input.SizeFlagsVertical = SizeFlags.ShrinkBegin;
        inputContainer.AddChild(input);

        AddOutputPort();

        // Create preview.
        Preview = new();
        Preview.Name = "Preview";
        Preview.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Preview.HorizontalAlignment = HorizontalAlignment.Center;
        Preview.VerticalAlignment = VerticalAlignment.Center;
        Preview.AddThemeFontSizeOverride("font_size", 10);
        Contents.AddChild(Preview);
        Preview.Hide();

        // Add space.
        Control space = new();
        Contents.AddChild(space);
    }

    /* Public methods. */
    /// <summary>
    /// Add an output port.
    /// </summary>
    public void AddOutputPort()
    {
        OutputPortContainer output = new();
        Outputs.Add(output);
        OutputContainer.AddChild(output);
    }

    /// <summary>
    /// Remove an output port.
    /// </summary>
    public void RemoveOutputPort(int index)
    {
        Outputs.RemoveAt(index);
        OutputContainer.RemoveChild(OutputContainer.GetChild(index));
    }

    /// <summary>
    /// Set the label text of an output port.
    /// </summary>
    public void SetOutputLabel(int index, string text) => Outputs[index].LabelText = text;
}
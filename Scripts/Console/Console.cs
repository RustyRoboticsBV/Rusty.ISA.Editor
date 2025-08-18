using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public partial class Console : MarginContainer
{
    /* Public methods. */
    public Font Font { get; set; }

    /* Private properties. */
    private static Color MessageColor { get; set; } = Color.FromHtml("#e0e0e0");
    private static Color WarningColor { get; set; }= Color.FromHtml("#ffde66");
    private static Color ErrorColor { get; set; } = Color.FromHtml("#ff786b");

    private Color OddColor { get; set; } = new Color(0.25f, 0.25f, 0.25f);
    private Color EvenColor { get; set; } = new Color(0.20f, 0.20f, 0.20f);

    private VBoxContainer LineContainer { get; set; }
    private List<ConsoleLine> Lines { get; } = new();

    /* Constructors. */
    public Console()
    {
        AddThemeConstantOverride("margin_left", 4);
        AddThemeConstantOverride("margin_right", 4);

        VBoxContainer vbox = new();
        AddChild(vbox);

        HBoxContainer buttons = new();
        vbox.AddChild(buttons);

        Label label = new();
        label.Text = "Console";
        buttons.AddChild(label);

        Button clearButton = new();
        clearButton.Text = "Clear";
        clearButton.Pressed += OnClearPressed;
        buttons.AddChild(clearButton);

        Button collapseButton = new();
        collapseButton.Text = "Collapse All";
        collapseButton.Pressed += OnCollapsePressed;
        buttons.AddChild(collapseButton);

        /*Label title = new();
        title.Text = "Console";
        vbox.AddChild(title);

        HSeparator separator = new();
        vbox.AddChild(separator);*/

        HBoxContainer hbox = new();
        vbox.AddChild(hbox);

        LineContainer = new();
        LineContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        LineContainer.AddThemeConstantOverride("separation", 0);
        hbox.AddChild(LineContainer);

        /*MarginContainer margin = new();
        margin.AddThemeConstantOverride("margin_left", 4);
        margin.AddThemeConstantOverride("margin_right", 4);
        hbox.AddChild(margin);*/
    }

    /* Public methods. */
    public void PrintMessage(string text)
    {
        Print(text, MessageColor);
    }

    public void PrintWarning(string text)
    {
        Print("WARNING: " + text, WarningColor);
    }

    public void PrintError(string text)
    {
        Print("ERROR: " + text, ErrorColor);
    }

    /* Private methods. */
    private void Print(string text, Color color)
    {
        ConsoleLine line = new();
        line.LabelText = text;
        line.LabelColor = color;
        line.LabelFont = Font;
        line.BackgroundColor = Lines.Count % 2 == 0 ? OddColor : EvenColor;
        line.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        LineContainer.AddChild(line);
        Lines.Add(line);
    }

    private void OnClearPressed()
    {
        foreach (ConsoleLine line in Lines)
        {
            LineContainer.RemoveChild(line);
        }
        Lines.Clear();
    }

    private void OnCollapsePressed()
    {
        foreach (ConsoleLine line in Lines)
        {
            line.Collapse();
        }
    }
}

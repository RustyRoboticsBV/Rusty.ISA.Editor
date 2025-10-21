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
        AddThemeConstantOverride("margin_bottom", 4);

        // Add vbox.
        VBoxContainer vbox = new();
        AddChild(vbox);

        // Add buttons.
        HBoxContainer buttons = new();
        buttons.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        vbox.AddChild(buttons);

        Label label = new();
        label.Text = "Console";
        label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        buttons.AddChild(label);

        Button expandButton = new();
        expandButton.Text = "Expand All";
        expandButton.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
        expandButton.Pressed += OnExpandPressed;
        buttons.AddChild(expandButton);

        Button collapseButton = new();
        collapseButton.Text = "Collapse All";
        collapseButton.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
        collapseButton.Pressed += OnCollapsePressed;
        buttons.AddChild(collapseButton);

        Button clearButton = new();
        clearButton.Text = "ClearGraph";
        clearButton.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
        clearButton.Pressed += OnClearPressed;
        buttons.AddChild(clearButton);

        // Add scroll.
        ScrollContainer scroll = new();
        scroll.SizeFlagsVertical = SizeFlags.ExpandFill;
        vbox.AddChild(scroll);

        VBoxContainer vbox2 = new();
        vbox2.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        scroll.AddChild(vbox2);

        LineContainer = new();
        LineContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        LineContainer.AddThemeConstantOverride("separation", 0);
        vbox2.AddChild(LineContainer);
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

    private void OnExpandPressed()
    {
        foreach (ConsoleLine line in Lines)
        {
            line.Expand();
        }
    }

    private void OnCollapsePressed()
    {
        foreach (ConsoleLine line in Lines)
        {
            line.Collapse();
        }
    }

    private void OnClearPressed()
    {
        foreach (ConsoleLine line in Lines)
        {
            LineContainer.RemoveChild(line);
        }
        Lines.Clear();
    }
}

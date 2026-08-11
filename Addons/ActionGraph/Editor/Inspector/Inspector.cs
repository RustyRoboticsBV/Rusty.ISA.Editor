using Godot;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// The inspector window.
/// </summary>
internal partial class Inspector : ScrollContainer
{
    public List<Panel> Panels { get; } = new();
    private VBoxContainer Vbox { get; set; }

    public Inspector()
    {
        Vbox = new();
        Vbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Vbox);

        ColorField color = new("Color1", Colors.Beige);
        Vbox.AddChild(color);

        ColorField color2 = color.CopyField();
        color2.TitleText = "Color2";
        color2.SetColor(Colors.ForestGreen);
        Vbox.AddChild(color2);

        Vbox.AddChild(new CheckBox());
        Vbox.AddChild(new CheckButton());
        Vbox.AddChild(new SpinBox());
        Vbox.AddChild(new HSlider());
        Vbox.AddChild(new LineEdit());
        Vbox.AddChild(new TextEdit());
        Vbox.AddChild(new OptionButton(["a", "b", "c"]));
        Vbox.AddChild(new ColorPickerButton());
    }

    public void Add(Panel panel)
    {
        Panels.Add(panel);
        AddChild(panel);
    }

    public void Remove(Panel panel)
    {
        Panels.Remove(panel);
        RemoveChild(panel);
    }
}
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

        BoolField @bool = new("Bool", false);
        Vbox.AddChild(@bool);

        BoolField bool2 = @bool.DuplicateField();
        bool2.SetPressed(true);
        Vbox.AddChild(@bool2);

        NumericField num = new("Num", 10, 0, 100, 0.1);
        Vbox.AddChild(num);

        NumericField num2 = num.DuplicateField();
        num2.SetValue(5);
        Vbox.AddChild(num2);

        ColorField color = new("Color1", Colors.Beige);
        Vbox.AddChild(color);

        ColorField color2 = color.DuplicateField();
        color2.TitleText = "Color2";
        color2.SetColor(Colors.ForestGreen);
        Vbox.AddChild(color2);

        EnumField @enum = new("Enum", ["A", "B", "C"], 1);
        Vbox.AddChild(@enum);

        EnumField enum2 = @enum.DuplicateField();
        enum2.TitleText = "Enum2";
        enum2.SetSelected(2);
        Vbox.AddChild(@enum2);

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
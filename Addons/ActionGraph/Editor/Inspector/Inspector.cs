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
        bool2.TitleText = "Bool2";
        bool2.SetPressed(true);
        Vbox.AddChild(@bool2);

        NumericField num = new("Num", 10, 0, 100, 0.1);
        Vbox.AddChild(num);

        NumericField num2 = num.DuplicateField();
        num2.TitleText = "Num2";
        num2.SetValue(5);
        Vbox.AddChild(num2);

        RangeField range = new("Range", 10, 0, 100, 1);
        Vbox.AddChild(range);

        RangeField range2 = range.DuplicateField();
        range2.TitleText = "Range2";
        range2.SetMin(20);
        range2.SetMax(50);
        range2.SetValue(25);
        Vbox.AddChild(range2);

        LineField line = new("Line", "abcdefg");
        Vbox.AddChild(line);

        LineField line2 = line.DuplicateField();
        line2.TitleText = "Line2";
        line2.SetText("cheese");
        Vbox.AddChild(line2);

        TextField text = new("Text", "ABCDEFG\nHIJKLMNOP");
        Vbox.AddChild(text);

        TextField text2 = text.DuplicateField();
        text2.TitleText = "Text2";
        text2.SetText("asdasdasdasd\nasdasd asdasd");
        Vbox.AddChild(text2);

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
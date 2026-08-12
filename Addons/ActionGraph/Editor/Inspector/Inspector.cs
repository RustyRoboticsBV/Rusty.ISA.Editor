using Godot;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// The inspector window.
/// </summary>
internal partial class Inspector : ScrollContainer
{
    public List<Panel> Panels { get; } = new();
    private VBoxContainer VBox { get; set; }

    public Inspector()
    {
        MarginContainer margin = new();
        margin.Name = "RightMargin";
        margin.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        margin.AddThemeConstantOverride("margin_right", 4);
        AddChild(margin);

        VBox = new();
        VBox.Name = "Contents";
        VBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        margin.AddChild(VBox);

        BoolField @bool = new("Bool", false);
        VBox.AddChild(@bool);

        BoolField bool2 = @bool.DuplicateField();
        bool2.TitleText = "Bool2";
        bool2.SetPressed(true);
        VBox.AddChild(@bool2);

        NumericField num = new("Num", 10, 0, 100, 0.1);
        VBox.AddChild(num);

        NumericField num2 = num.DuplicateField();
        num2.TitleText = "Num2";
        num2.SetValue(5);
        VBox.AddChild(num2);

        RangeField range = new("Range", 10, 0, 100, 1);
        VBox.AddChild(range);

        RangeField range2 = range.DuplicateField();
        range2.TitleText = "Range2";
        range2.SetMin(20);
        range2.SetMax(50);
        range2.SetValue(25);
        VBox.AddChild(range2);

        LineField line = new("Line", "abcdefg");
        VBox.AddChild(line);

        LineField line2 = line.DuplicateField();
        line2.TitleText = "Line2";
        line2.SetText("cheese");
        VBox.AddChild(line2);

        TextField text = new("Text", "ABCDEFG\nHIJKLMNOP");
        VBox.AddChild(text);

        TextField text2 = text.DuplicateField();
        text2.TitleText = "Text2";
        text2.SetText("asdasdasdasd\nasdasd asdasd");
        VBox.AddChild(text2);

        ColorField color = new("Color1", Colors.Beige);
        VBox.AddChild(color);

        ColorField color2 = color.DuplicateField();
        color2.TitleText = "Color2";
        color2.SetColor(Colors.ForestGreen);
        VBox.AddChild(color2);

        EnumField @enum = new("Enum", ["A", "B", "C"], 1);
        VBox.AddChild(@enum);

        EnumField enum2 = @enum.DuplicateField();
        enum2.TitleText = "Enum2";
        enum2.SetSelected(2);
        VBox.AddChild(@enum2);

        /*ListElement element = new(enum2.DuplicateField());
        element.PressedInsert += (element) => GD.Print("Pressed insert: " + element);
        element.PressedDuplicate += (element) => GD.Print("Pressed duplicate: " + element);
        element.PressedDelete += (element) => GD.Print("Pressed delete: " + element);
        element.MovedUp += (element) => GD.Print("Moved up: " + element);
        element.MovedDown += (element) => GD.Print("Moved down: " + element);
        VBox.AddChild(element);*/

        ListContainer list = new("List", "Enum Element", enum2, "Add Enum Element");
        VBox.AddChild(list);

        VBox.AddChild(new CheckBox());
        VBox.AddChild(new CheckButton());
        VBox.AddChild(new SpinBox());
        VBox.AddChild(new HSlider());
        VBox.AddChild(new LineEdit());
        VBox.AddChild(new TextEdit());
        VBox.AddChild(new OptionButton(["a", "b", "c"]));
        VBox.AddChild(new ColorPickerButton());
    }

    public void Add(Panel panel)
    {
        Panels.Add(panel);
        VBox.AddChild(panel);
    }

    public void Remove(Panel panel)
    {
        Panels.Remove(panel);
        VBox.RemoveChild(panel);
    }
}
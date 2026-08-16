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

        BoolField bool2 = @bool.DuplicateWidget();
        bool2.TitleText = "Bool2";
        bool2.SetPressed(true);
        VBox.AddChild(@bool2);

        NumericField num = new("Num", 10, 0, 100, 0.1);
        VBox.AddChild(num);

        NumericField num2 = num.DuplicateWidget();
        num2.TitleText = "Num2";
        num2.SetValue(5);
        VBox.AddChild(num2);

        RangeField range = new("Range", 10, 0, 100, 1);
        VBox.AddChild(range);

        RangeField range2 = range.DuplicateWidget();
        range2.TitleText = "Range2";
        range2.SetMin(20);
        range2.SetMax(50);
        range2.SetValue(25);
        VBox.AddChild(range2);

        LineField line = new("Line", "abcdefg");
        VBox.AddChild(line);

        LineField line2 = line.DuplicateWidget();
        line2.TitleText = "Line2";
        line2.SetText("cheese");
        VBox.AddChild(line2);

        TextField text = new("Text", "ABCDEFG\nHIJKLMNOP");
        VBox.AddChild(text);

        TextField text2 = text.DuplicateWidget();
        text2.TitleText = "Text2";
        text2.SetText("asdasdasdasd\nasdasd asdasd");
        VBox.AddChild(text2);

        ColorField color = new("Color1", Colors.Beige);
        VBox.AddChild(color);

        ColorField color2 = color.DuplicateWidget();
        color2.TitleText = "Color2";
        color2.SetColor(Colors.ForestGreen);
        VBox.AddChild(color2);

        EnumField @enum = new("Enum", ["A", "B", "C"], 1);
        VBox.AddChild(@enum);

        EnumField enum2 = @enum.DuplicateWidget();
        enum2.TitleText = "Enum2";
        enum2.SetSelected(2);
        VBox.AddChild(@enum2);

        OptionWidget option = new("Option", enum2.DuplicateWidget(), false);
        VBox.AddChild(option);

        ChoiceWidget choice = new("Choice", new() { { "Enum", @enum.DuplicateWidget() }, { "Color", color.DuplicateWidget() }, { "Option", option.DuplicateWidget() } }, 1);
        VBox.AddChild(choice);

        Label label = new();
        label.Text = "TUPLE";
        VBox.AddChild(label);

        TupleWidget tuple = new("Tuple", [option.DuplicateWidget(), choice.DuplicateWidget()]);
        VBox.AddChild(tuple);

        ListWidget list = new("List", "Tuple Element", tuple.DuplicateWidget(), "Add Tuple");
        VBox.AddChild(list);

        ListWidget listList = new("Nested List", "Nested List", list.DuplicateWidget(), "Add Nested");
        VBox.AddChild(listList);
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
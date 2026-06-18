using Godot;

namespace Rusty.ISA;

[GlobalClass]
public partial class Test : ScrollContainer
{
    public override void _EnterTree()
    {
        FoldoutGroup foldout = new();
        foldout.Title = "COOL FOLDOUT";
        foldout.Description = "THIS IS A COOL FOLDOUT";
        foldout.Indentation = 40;
        foldout.ExpandFill(true);
        AddChild(foldout);

        VBoxGroup vbox = new();
        vbox.Title = "COOL VBOX";
        vbox.Description = "THIS IS A COOL VBOX.";
        vbox.Indentation = 40;
        vbox.ExpandFill(true);
        foldout.AddWidget(vbox);

        OptionalGroup togglebox = new();
        togglebox.Title = "COOL TOGGLEBOX";
        togglebox.Description = "THIS IS A SICK TOGGLEBOX";
        togglebox.Indentation = 20;
        togglebox.ExpandFill();
        vbox.AddWidget(togglebox);

        ToggleField toggle = new();
        toggle.Title = "COOL TOGGLE";
        toggle.Description = "THIS IS A SICK TOGGLE.";
        toggle.Value = true;
        toggle.ExpandFill();
        togglebox.AddWidget(toggle);

        NumericField number = new();
        number.Title = "COOL NUMBER";
        number.Description = "THIS IS A SICK NUMBER.";
        number.Value = 10f;
        number.ExpandFill();
        togglebox.AddWidget(number);

        togglebox.Value = true;

        TextLineField line = new();
        line.Title = "COOL TEXT LINE";
        line.Description = "THIS IS A SICK TEXT LINE.";
        line.Value = "ABC";
        line.ExpandFill();
        togglebox.AddWidget(line);

        TextAreaField area = new();
        area.Title = "COOL TEXT AREA";
        area.Description = "THIS IS A SICK TEXT AREA.";
        area.Value = "ABC\nDEF";
        area.ExpandFill();
        togglebox.AddWidget(area);

        ColorField color = new();
        color.Title = "COOL COLOR";
        color.Description = "THIS IS A SICK COLOR.";
        color.Value = Colors.Red;
        color.ExpandFill();
        togglebox.AddWidget(color);

        EnumField enums = new();
        enums.Title = "COOL ENUM    ";
        enums.Description = "THIS IS A SICK ENUM.";
        enums.Choices = ["A", "B", "C"];
        enums.Value = 1;
        enums.ExpandFill();
        togglebox.AddWidget(enums);

        HBoxGroup hbox = new();
        hbox.Title = "COOL HBOX";
        hbox.Description = "THIS IS A SICK HBOX.";
        hbox.Indentation = 10;
        hbox.ExpandFill();
        vbox.AddWidget(hbox);

        IWidget x = number.Copy();
        x.Title = "X";
        x.TitleWidth = 40;
        x.ExpandFill();
        hbox.AddWidget(x);

        IWidget y = x.Copy();
        y.Title = "Y";
        hbox.AddWidget(y);

        IGroup hboxcopy = vbox.Copy() as IGroup;
        vbox.AddWidget(hboxcopy);
        (hboxcopy.GetWidgetAt(0) as OptionalGroup).Value = false;

        foldout.AddWidget(foldout.Copy());
    }
}
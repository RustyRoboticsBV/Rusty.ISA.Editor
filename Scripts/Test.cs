using Godot;
using Rusty.ISA.Consoles;

namespace Rusty.ISA;

[GlobalClass]
public partial class Test : ScrollContainer
{
    private UndoRedo UndoRedo { get; set; } = new();
    private bool UndoRedoPressed { get; set; }

    public override void _EnterTree()
    {
        FoldoutGroup foldout = new();
        foldout.Title = "COOL FOLDOUT";
        foldout.Description = "THIS IS A COOL FOLDOUT";
        foldout.Indentation = 40;
        foldout.ExpandFill(true);
        foldout.UndoRedo = UndoRedo;
        AddChild(foldout);

        VBoxGroup vbox = new();
        vbox.Title = "COOL VBOX";
        vbox.Description = "THIS IS A COOL VBOX.";
        vbox.Indentation = 40;
        vbox.ExpandFill(true);
        vbox.UndoRedo = UndoRedo;
        foldout.AddWidget(vbox);

        OptionalGroup togglebox = new();
        togglebox.Title = "COOL TOGGLEBOX";
        togglebox.Description = "THIS IS A SICK TOGGLEBOX";
        togglebox.Indentation = 20;
        togglebox.ExpandFill();
        togglebox.UndoRedo = UndoRedo;
        vbox.AddWidget(togglebox);

        ToggleField toggle = new();
        toggle.Title = "COOL TOGGLE";
        toggle.Description = "THIS IS A SICK TOGGLE.";
        toggle.Value = true;
        toggle.ExpandFill();
        toggle.UndoRedo = UndoRedo;
        togglebox.AddWidget(toggle);

        NumericField number = new();
        number.Title = "COOL NUMBER";
        number.Description = "THIS IS A SICK NUMBER.";
        number.Value = 10f;
        number.ExpandFill();
        number.UndoRedo = UndoRedo;
        togglebox.AddWidget(number);

        togglebox.Value = true;

        TextLineField line = new();
        line.Title = "COOL TEXT LINE";
        line.Description = "THIS IS A SICK TEXT LINE.";
        line.Value = "ABC";
        line.ExpandFill();
        line.UndoRedo = UndoRedo;
        togglebox.AddWidget(line);

        TextAreaField area = new();
        area.Title = "COOL TEXT AREA";
        area.Description = "THIS IS A SICK TEXT AREA.";
        area.Value = "ABC\nDEF";
        area.ExpandFill();
        area.UndoRedo = UndoRedo;
        togglebox.AddWidget(area);

        ColorField color = new();
        color.Title = "COOL COLOR";
        color.Description = "THIS IS A SICK COLOR.";
        color.SetValue(Colors.Red);
        color.ExpandFill();
        color.UndoRedo = UndoRedo;
        togglebox.AddWidget(color);

        EnumField enums = new();
        enums.Title = "COOL ENUM    ";
        enums.Description = "THIS IS A SICK ENUM.";
        enums.SetChoices(["A", "B", "C"]);
        enums.SetValue(1);
        enums.ExpandFill();
        enums.UndoRedo = UndoRedo;
        togglebox.AddWidget(enums);

        HBoxGroup hbox = new();
        hbox.Title = "COOL HBOX";
        hbox.Description = "THIS IS A SICK HBOX.";
        hbox.Indentation = 10;
        hbox.ExpandFill();
        hbox.UndoRedo = UndoRedo;
        vbox.AddWidget(hbox);

        IWidget x = number.Copy();
        x.Title = "X";
        x.TitleWidth = 40;
        x.ExpandFill();
        x.UndoRedo = UndoRedo;
        hbox.AddWidget(x);

        IWidget y = x.Copy();
        y.Title = "Y";
        y.UndoRedo = UndoRedo;
        hbox.AddWidget(y);

        IGroup hboxcopy = vbox.Copy() as IGroup;
        vbox.AddWidget(hboxcopy);
        vbox.UndoRedo = UndoRedo;
        (hboxcopy.GetWidgetAt(0) as OptionalGroup).Value = false;

        foldout.AddWidget(foldout.Copy());
    }

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Z) && Input.IsKeyPressed(Key.Ctrl))
        {
            if (!UndoRedoPressed)
            {
                UndoRedoPressed = true;
                if (Input.IsKeyPressed(Key.Shift))
                {
                    if (UndoRedo.HasRedo())
                    {
                        UndoRedo.Redo();
                        Log.Message(UndoRedo.GetCurrentActionName());
                    }
                }
                else if (UndoRedo.HasUndo())
                    UndoRedo.Undo();
            }
        }
        else
            UndoRedoPressed = false;
    }
}
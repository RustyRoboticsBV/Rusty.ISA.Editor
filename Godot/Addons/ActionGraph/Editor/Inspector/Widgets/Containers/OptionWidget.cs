using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An option widget.
/// </summary>
internal sealed partial class OptionWidget : VBoxContainer, IWidget<OptionWidget>
{
    /* Public properties. */
    public string TitleText
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    public bool Enabled => CheckBox.ButtonPressed;
    public Control Optional { get; private set; }
    public int Indentation
    {
        get => Margin.GetThemeConstant("margin_left");
        set => Margin.AddThemeConstantOverride("margin_left", value);
    }
    public UndoRedo UndoRedo
    {
        get => CheckBox.UndoRedo;
        set
        {
            CheckBox.UndoRedo = value;
            if (Optional is IWidget widget)
                widget.UndoRedo = value;
        }
    }

    /* Private properties. */
    private CheckBox CheckBox { get; set; }
    private Label Label { get; set; }
    private MarginContainer Margin { get; set; }

    /* Public methods. */
    public event Action StateChanged;

    /* Constructors. */
    public OptionWidget(string titleText, Control optional, bool enabled)
    {
        MouseFilter = MouseFilterEnum.Pass;
        SizeFlagsHorizontal = SizeFlags.ExpandFill;

        // Add checkbox.
        HBoxContainer hbox = new();
        AddChild(hbox);

        CheckBox = new();
        CheckBox.ButtonPressed = enabled;
        CheckBox.Pressed += () => StateChanged?.Invoke();
        CheckBox.Toggled += (toggledOn) => StateChanged?.Invoke();
        hbox.AddChild(CheckBox);

        Label = new();
        Label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Label.Text = titleText;
        hbox.AddChild(Label);

        // Add optional.
        Margin = new();
        Margin.Name = "OptionalMargin";
        AddChild(Margin);
        Indentation = 28;

        Optional = optional;
        Margin.AddChild(optional);

        if (Optional is IWidget widget)
            widget.StateChanged += () => StateChanged?.Invoke();
    }

    /* Public methods. */
    IWidget IWidget.DuplicateWidget() => DuplicateWidget();

    public OptionWidget DuplicateWidget()
    {
        Control optional = Optional is IWidget widget
            ? widget.DuplicateWidget() as Control
            : Optional.Duplicate() as Control;

        OptionWidget copy = new(TitleText, optional, Enabled);
        copy.SizeFlagsHorizontal = SizeFlagsHorizontal;
        copy.SizeFlagsVertical = SizeFlagsVertical;
        copy.Indentation = Indentation;
        copy.UndoRedo = UndoRedo;

        return copy;
    }

    public void SetEnabled(bool enabled)
    {
        CheckBox.ButtonPressed = enabled;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        if (Optional != null)
            Optional.Visible = Enabled;
    }
}
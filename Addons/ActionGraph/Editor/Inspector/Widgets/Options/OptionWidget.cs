using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An option widget.
/// </summary>
internal sealed partial class OptionWidget : VBoxContainer, IWidget
{
    /* Public properties. */
    public string TitleText { get; set; }
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
        set => CheckBox.UndoRedo = value;
    }

    /* Private properties. */
    private CheckBox CheckBox { get; set; }
    private Label Label { get; set; }
    private MarginContainer Margin { get; set; }

    /* Constructors. */
    public OptionWidget(bool enabled, string titleText, Control optional)
    {
        TitleText = titleText;
        MouseFilter = MouseFilterEnum.Pass;
        SizeFlagsHorizontal = SizeFlags.ExpandFill;

        // Add checkbox.
        HBoxContainer hbox = new();
        AddChild(hbox);

        CheckBox = new();
        CheckBox.ButtonPressed = enabled;
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
    }

    /* Public methods. */
    IWidget IWidget.DuplicateWidget() => DuplicateWidget();

    public OptionWidget DuplicateWidget()
    {
        Control optional = Optional is IWidget widget
            ? widget.DuplicateWidget() as Control
            : Optional.Duplicate() as Control;

        OptionWidget copy = new(Enabled, TitleText, optional);
        copy.SizeFlagsHorizontal = SizeFlagsHorizontal;
        copy.SizeFlagsVertical = SizeFlagsVertical;
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
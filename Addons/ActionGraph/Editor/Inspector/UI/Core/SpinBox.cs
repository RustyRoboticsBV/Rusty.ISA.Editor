using Godot;
using static System.Net.Mime.MediaTypeNames;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A SpinBox that suppresses built-in undo/redo behavior.
/// </summary>
public sealed partial class SpinBox : Godot.SpinBox
{
    /* Public properties. */
    public UndoRedo UndoRedo { get; set; }

    /* Private properties. */
    private double LastValue { get; set; }

    /* Constructors. */
    public SpinBox() : base()
    {
        GetLineEdit().FocusEntered += OnFocusEntered;
        FocusEntered += OnFocusEntered;

        GetLineEdit().FocusExited += OnFocusExited;
        FocusExited += OnFocusExited;
    }

    /* Public methods. */
    public new void SetValue(double value)
    {
        Value = value;
        LastValue = value;
    }

    public void CommitValue(double value)
    {
        GD.Print("Checking " + value + " against " + LastValue);
        if (value != LastValue)
        {
            Record(LastValue, value);
            Value = value;
            LastValue = value;
        }
    }

    /* Godot overrides. */
    public override void _Input(InputEvent @event)
    {
        if (!GetLineEdit().HasFocus())
            return;

        if (@event is InputEventKey key)
        {
            GD.Print("KEY " + key.Keycode);
            if (key.Pressed)
            {
                if (key.CtrlPressed && key.Keycode == Key.Z)
                    AcceptEvent();
                else if (key.CtrlPressed && key.Keycode == Key.Y)
                    AcceptEvent();
                else if (!key.CtrlPressed && key.Keycode == Key.Escape)
                {
                    GetLineEdit().Text = LastValue.ToString();
                    GetLineEdit().ReleaseFocus();
                    ReleaseFocus();
                    AcceptEvent();
                }
                else if (!key.CtrlPressed && key.Keycode == Key.Enter)
                {
                    GetLineEdit().ReleaseFocus();
                    ReleaseFocus();
                    AcceptEvent();
                }
            }
        }
    }

    /* Private methods. */
    private void OnFocusEntered()
    {
        LastValue = Value;
    }

    private void OnFocusExited()
    {
        GD.Print("TEXT " + GetLineEdit().Text);
        if (double.TryParse(GetLineEdit().Text, out double parsed))
            CommitValue(parsed);
        LastValue = Value;
    }

    private void Record(double from, double to)
    {
        GD.Print("HOLY FUCKING SHIT " + from + " => " + to);
        if (UndoRedo == null || from == to)
            return;

        UndoRedo.CreateAction($"Changed SpinBox '{Name}': {from} => {to}");

        UndoRedo.AddUndoProperty(this, "value", from);

        UndoRedo.AddDoProperty(this, "value", to);

        UndoRedo.CommitAction(false);
    }
}
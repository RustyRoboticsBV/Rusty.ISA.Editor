using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An OptionButton that suppresses built-in undo/redo behavior.
/// </summary>
public sealed partial class OptionButton : Godot.OptionButton
{
    /* Public properties. */
    public UndoRedo UndoRedo { get; set; }

    /* Private properties. */
    private int LastSelected { get; set; }

    /* Constructors. */
    public OptionButton() : base()
    {
        GetPopup().AboutToPopup += OnPopupAboutToPopup;
        ItemSelected += OnItemSelected;
    }

    public OptionButton(string[] options) : this()
    {
        foreach (string option in options)
        {
            AddItem(option);
        }
    }

    /* Godot overrides. */
    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventKey key)
        {
            if (key.Pressed)
            {
                if (key.CtrlPressed && key.Keycode == Key.Z)
                    AcceptEvent();
                else if (key.CtrlPressed && key.Keycode == Key.Y)
                    AcceptEvent();
                else if (!key.CtrlPressed && key.Keycode == Key.Escape)
                {
                    GetPopup().Hide();
                    ReleaseFocus();
                    AcceptEvent();
                }
            }
            else if (!key.CtrlPressed && key.Keycode == Key.Escape)
            {
                GetPopup().Hide();
                ReleaseFocus();
                GetViewport().SetInputAsHandled();
            }
        }
    }

    /* Private methods. */
    private void OnPopupAboutToPopup()
    {
        LastSelected = Selected;
    }

    private void OnItemSelected(long index)
    {
        Record(LastSelected, (int)index);
        ReleaseFocus();
    }

    private void Record(int from, int to)
    {
        if (UndoRedo == null || from == to)
            return;

        UndoRedo.CreateAction($"Changed OptionButton '{Name}': {from} => {to}");

        UndoRedo.AddUndoProperty(this, "selected", from);

        UndoRedo.AddDoProperty(this, "selected", to);

        UndoRedo.CommitAction(false);
    }
}
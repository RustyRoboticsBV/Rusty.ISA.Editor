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

    public OptionButton(string[] items) : this()
    {
        foreach (string item in items)
        {
            AddItem(item);
        }
    }

    /* Public methods. */
    public void SetItems(string[] items)
    {
        while (ItemCount > 0)
        {
            RemoveItem(0);
        }
        foreach (string item in items)
        {
            AddItem(item);
        }
    }

    public string[] GetItems()
    {
        string[] items = new string[ItemCount];
        for (int i = 0; i < ItemCount; i++)
        {
            items[i] = GetItemText(i);
        }
        return items;
    }

    public new void Select(int index)
    {
        Selected = index;
        LastSelected = index;
    }

    public void CommitSelect(int index)
    {
        GD.Print(LastSelected + "  " + index);
        if (LastSelected != index)
        {
            Record(LastSelected, index);
            Selected = index;
            LastSelected = index;
        }
    }

    public void CancelSelect()
    {
        GetPopup().Hide();
        ReleaseFocus();
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
                    CancelSelect();
                    AcceptEvent();
                }
            }
            else if (!key.CtrlPressed && key.Keycode == Key.Escape)
            {
                CancelSelect();
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
        CommitSelect((int)index);
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
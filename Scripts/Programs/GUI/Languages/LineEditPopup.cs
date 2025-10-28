using Godot;
using System;
using static Godot.Control;

namespace Rusty.ISA.Editor;

public partial class LineEditPopup : ConfirmationDialog
{
    /* Public properties. */
    public string Text
    {
        get => LineEdit.Text;
        set => LineEdit.Text = value;
    }

    /* Private properties. */
    private LineEdit LineEdit { get; set; }

    /* Public events. */
    public new event Action<string> Confirmed;
    public new event Action Canceled;

    /* Constructors. */
    public LineEditPopup()
    {
        LineEdit = new();
        Size = new(512, Size.Y);
        KeepTitleVisible = true;
        ExtendToTitle = true;

        base.Confirmed += OnConfirmed;
        base.Canceled += OnCanceled;
        base.VisibilityChanged += OnVisibilityChanged;

        LineEdit = new();
        LineEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        LineEdit.CustomMinimumSize = new(Size.X - 8, 0);
        LineEdit.TextSubmitted += OnTextSubmitted;
        LineEdit.TextChangeRejected += OnTextRejected;
        GetChild(0, true).AddChild(LineEdit);
    }

    /* Private methods. */
    private void OnConfirmed()
    {
        Confirmed?.Invoke(Text);
    }

    private void OnTextSubmitted(string text)
    {
        Confirmed?.Invoke(Text);
    }

    private void OnCanceled()
    {
        Canceled?.Invoke();
    }

    private void OnTextRejected(string text)
    {
        Canceled?.Invoke();
    }

    private void OnVisibilityChanged()
    {
        if (Visible)
            LineEdit.GrabFocus();
    }
}
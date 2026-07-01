using Godot;
using System;

namespace Rusty.ISA;

/// <summary>
/// A numeric field.
/// </summary>
public sealed partial class NumericField : HBoxContainer, IWidget, IValued<double>
{
    /* Public properties. */
    public string Title
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    public int TitleWidth
    {
        get => (int)Label.CustomMinimumSize.X;
        set => Label.CustomMinimumSize = new(value, Label.CustomMinimumSize.Y);
    }
    public string Description
    {
        get => TooltipText;
        set
        {
            TooltipText = value;
            Label.TooltipText = value;
            SpinBox.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo { get; set; }

    public double Value => SpinBox.Value;

    public double Step => SpinBox.Step;
    public double MinValue => SpinBox.MinValue;
    public double MaxValue => SpinBox.MaxValue;

    /* Private methods. */
    private Label Label { get; set; }
    private SpinBox SpinBox { get; set; }
    private double OldValue { get; set; }
    private bool Cancelled { get; set; }

    /* Public events. */
    public event Action<IWidget> Changed;

    /* Constructors. */
    public NumericField()
    {
        Label = new();
        AddChild(Label);
        TitleWidth = 160;

        SpinBox = new();
        AddChild(SpinBox);
        SpinBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        SpinBox.GetLineEdit().ContextMenuEnabled = false;
        SpinBox.GetLineEdit().ShortcutKeysEnabled = false;
        SpinBox.GetLineEdit().FocusEntered += OnSelected;
        SpinBox.GetLineEdit().TextSubmitted += OnSubmitted;
        SpinBox.GetLineEdit().FocusExited += OnSubmitted;

        WidgetRegistry.Add(this);
    }

    public NumericField(double value) : this()
    {
        SpinBox.Value = value;
        OldValue = SpinBox.Value;
    }

    public NumericField(double value, double min, double max, double step) : this()
    {
        SpinBox.MinValue = min;
        SpinBox.MaxValue = max;
        SpinBox.Step = step;
        SpinBox.Value = value;
        OldValue = SpinBox.Value;
    }

    ~NumericField()
    {
        WidgetRegistry.Remove(this);
    }

    /* Public methods. */
    public IWidget Copy()
    {
        NumericField field = new();
        field.SizeFlagsHorizontal = SizeFlagsHorizontal;
        field.SizeFlagsVertical = SizeFlagsVertical;
        field.Title = Title;
        field.TitleWidth = TitleWidth;
        field.Description = Description;
        field.SpinBox.Value = SpinBox.Value;
        field.SpinBox.Step = SpinBox.Step;
        field.SpinBox.MinValue = SpinBox.MinValue;
        field.SpinBox.MaxValue = SpinBox.MaxValue;
        field.UndoRedo = UndoRedo;
        field.OldValue = Value;
        return field;
    }

    public void ExpandFill(bool vertical = false)
    {
        SizeFlagsHorizontal = SizeFlags.ExpandFill;
        if (vertical)
            SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    public void SetValue(double value)
    {
        SetValue(SpinBox.Value, value);
        OldValue = value;
    }

    public void CancelFocus()
    {
        if (SpinBox.GetLineEdit().HasFocus())
        {
            Cancelled = true;
            SpinBox.GetLineEdit().ReleaseFocus();
            SpinBox.GetLineEdit().Text = OldValue.ToString();
            Cancelled = false;
        }
    }

    /* Private methods. */
    private void OnSelected()
    {
        OldValue = SpinBox.Value;
    }

    private void OnSubmitted(string value)
    {
        if (Cancelled)
            return;

        SetValue(OldValue, double.Parse(value));
    }

    private void OnSubmitted()
    {
        if (Cancelled)
            return;

        double value = Mathf.Clamp(double.Parse(SpinBox.GetLineEdit().Text), SpinBox.MinValue, SpinBox.MaxValue); ;
        SetValue(OldValue, value);
    }

    private void SetValue(double from, double to)
    {
        if (from == to)
            return;

        UndoRedo?.CreateAction($"Changed color {Title}: {from} \u25B6 {to}");

        UndoRedo?.AddUndoMethod(new Callable(this, nameof(CancelAll)));
        UndoRedo?.AddUndoProperty(SpinBox, "value", from);
        UndoRedo?.AddUndoMethod(new Callable(this, nameof(InvokeChangedEvent)));

        UndoRedo?.AddDoMethod(new Callable(this, nameof(CancelAll)));
        UndoRedo?.AddDoProperty(SpinBox, "value", to);
        UndoRedo?.AddDoMethod(new Callable(this, nameof(InvokeChangedEvent)));

        UndoRedo?.CommitAction(false);

        SpinBox.Value = to;
        InvokeChangedEvent();
    }

    private void CancelAll()
    {
        WidgetRegistry.ReleaseFocus();
    }

    private void InvokeChangedEvent()
    {
        Changed?.Invoke(this);
    }
}
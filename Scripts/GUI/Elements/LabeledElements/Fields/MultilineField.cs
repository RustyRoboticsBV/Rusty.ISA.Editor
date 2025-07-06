using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A string multiline-edit field.
/// </summary>
public partial class MultilineField : LabeledElement, Field<string>
{
    /* Public properties. */
    object Field.Value
    {
        get => Value;
        set => Value = (string)value;
    }
    public string Value
    {
        get => Field.Text;
        set => Field.Text = value;
    }

    /* Private properties. */
    private VBoxContainer VBoxContainer { get; set; }
    private TextEdit Field { get; set; }

    /* Public methods. */
    public override MultilineField Copy()
    {
        MultilineField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(MultilineField other)
    {
        CopyFrom(other as LabeledElement);
        Value = other.Value;
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add VBoxContainer.
        VBoxContainer = new();
        VBoxContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(VBoxContainer);

        // Move label.
        RemoveChild(Label);
        VBoxContainer.AddChild(Label);

        // Add line edit.
        Field = new();
        Field.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Field.CustomMinimumSize = new(0f, 128f);
        VBoxContainer.AddChild(Field);
    }
}
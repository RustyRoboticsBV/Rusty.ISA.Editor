namespace Rusty.ISA.Editor;

/// <summary>
/// An interface for an option field.
/// </summary>
public interface ChoiceField : Field
{
    /// <summary>
    /// The available options that the user can select.
    /// </summary>
    public string[] Options { get; set; }
    /// <summary>
    /// The currently-selected option.
    /// </summary>
    public int Selected { get; set; }
}

/// <summary>
/// An interface for an option field.
/// </summary>
public interface OptionField<T> : Field<T>, ChoiceField { }
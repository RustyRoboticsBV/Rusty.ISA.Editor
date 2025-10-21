using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// An instance of a preview evaluation script.
/// </summary>
public abstract partial class PreviewInstance : Resource
{
    /* Public properties. */
    public Preview Preview { get; private set; }
    public PreviewInput Input { get; private set; }

    /* Private properties. */
    private GodotObject Instance { get; set; }

    /* Constructors. */
    public PreviewInstance(Preview preview)
    {
        Preview = preview;

        // Copy defaults.
        Input = Preview.DefaultInput.Copy();
    }

    /* Public methods. */
    public override string ToString()
    {
        return Input.ToString();
    }

    /// <summary>
    /// Make a deep copy of this preview instance.
    /// </summary>
    public abstract PreviewInstance Copy();

    /// <summary>
    /// Evaluate the preview, using its current input.
    /// </summary>
    public string Evaluate()
    {
        if (Instance == null)
            Instance = Preview.Emit();

        return (string)Instance.Call("eval", Input);
    }

    /// <summary>
    /// Set the display name.
    /// </summary>
    public void SetDisplayName(string value)
    {
        Input.SetValue(Preview.DisplayName, value);
    }
}
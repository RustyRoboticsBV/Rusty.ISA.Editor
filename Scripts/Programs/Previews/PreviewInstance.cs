using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// An instance of a preview evaluation script.
/// </summary>
public abstract partial class PreviewInstance : Resource
{
    /* Public properties. */
    public Preview Preview { get; private set; }
    public PreviewInput Input { get; private set; } = new();

    /* Private properties. */
    private GodotObject Instance { get; set; }

    /* Constructors. */
    public PreviewInstance(Preview preview)
    {
        Preview = preview;
    }

    /* Public methods. */
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
}
namespace Rusty.ISA.Editor;

public abstract partial class ResourceInspector : Inspector
{
    /* Public properties. */
    public InstructionSet InstructionSet { get; private set; }
    public PreviewInstance Preview { get; protected set; }

    /* Private properties. */
    private bool PreviewEnabled { get; set; } = false;

    /* Constructors. */
    public ResourceInspector(InstructionSet instructionSet) : base()
    {
        // Store instruction set.
        InstructionSet = instructionSet;

        // Subscribe to events.
        Changed += OnChanged;
    }

    /* Public methods. */
    public override void CopyFrom(IGuiElement other)
    {
        // Copying invalidates the preview, so we set it to null.
        Preview = null;

        // Base inspector copy.
        base.CopyFrom(other);

        // Copy resource inspector data.
        if (other is ResourceInspector inspector)
            InstructionSet = inspector.InstructionSet;
    }

    /// <summary>
    /// Disable & clear the preview for this inspector.
    /// </summary>
    public void DisablePreview()
    {
        PreviewEnabled = false;
        Preview = null;
    }

    /// <summary>
    /// Enable & recompute the preview for this inspector.
    /// </summary>
    public void EnablePreview()
    {
        PreviewEnabled = true;
        UpdatePreview();
    }

    /* Protected methods. */
    /// <summary>
    /// Init and/or update the preview for this inspector.
    /// </summary>
    protected virtual void UpdatePreview() { }

    /* Private methods. */
    private void OnChanged()
    {
        if (PreviewEnabled)
            UpdatePreview();
    }
}
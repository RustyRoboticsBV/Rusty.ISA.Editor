namespace Rusty.ISA.Editor;

/// <summary>
/// An interface for containers.
/// </summary>
public interface IContainer : IGuiElement
{
    /* Public methods. */
    /// <summary>
    /// Get the number of elements inside of this container.
    /// </summary>
    public int GetContentsCount();

    /// <summary>
    /// Get an element in this container.
    /// </summary>
    public IGuiElement GetFromContents(int index);

    /// <summary>
    /// Add an element to this container.
    /// </summary>
    public void AddToContents(IGuiElement element);

    /// <summary>
    /// Remove an element from this container.
    /// </summary>
    public void RemoveFromContents(IGuiElement element);

    /// <summary>
    /// Delete all contents.
    /// </summary>
    public void ClearContents();
}
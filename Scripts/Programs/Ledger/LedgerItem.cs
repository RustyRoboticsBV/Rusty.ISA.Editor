using System;

namespace Rusty.ISA.Editor;

/// <summary>
/// An element-inspector pair.
/// </summary>
public abstract class LedgerItem
{
    /* Public properties. */
    public InstructionSet Set { get; private set; }
    public IGraphElement Element { get; private set; }
    public Inspector Inspector { get; private set; }

    /* Public events. */
    public event Action<LedgerItem> ElementSelected;
    public event Action<LedgerItem> ElementDeselected;
    public event Action<LedgerItem> ElementDeleted;

    /* Constructors. */
    public LedgerItem(InstructionSet set, IGraphElement element, Inspector inspector)
    {
        Set = set;
        Element = element;
        Inspector = inspector;

        //element.NodeSelected += OnElementSelected;
        //element.NodeDeselected += OnElementDeselected;
        //element.DeleteRequest += OnElementDeleted;
        inspector.Changed += OnInspectorChanged;
    }

    /* Protected methods. */
    protected abstract void OnInspectorChanged();

    /* Private methods. */
    private void OnElementSelected(IGraphElement element)
    {
        ElementSelected?.Invoke(this);
    }

    private void OnElementDeselected(IGraphElement element)
    {
        ElementDeselected?.Invoke(this);
    }

    private void OnElementDeleted(IGraphElement element)
    {
        ElementDeleted?.Invoke(this);
    }
}
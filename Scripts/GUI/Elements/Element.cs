using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A base class for elements.
/// </summary>
public abstract partial class Element : HBoxContainer
{
    /* Public methods. */
    /// <summary>
    /// The local indentation of this element.
    /// </summary>
    public int LocalIndentation { get; set; }
    /// <summary>
    /// The global indentation of this element.
    /// </summary>
    public int GlobalIndentation
    {
        get
        {
            Node parent = GetParent();
            while (parent != null)
            {
                if (parent is Container)
                    parent = parent.GetParent();
                else if (parent is Element element)
                    return element.GlobalIndentation;
                else
                    break;
            }
            return LocalIndentation;
        }
    }

    /* Private properties. */
    private Control Indentation { get; set; }
    private bool Initialized { get; set; }

    /* Constructors. */
    public Element()
    {
        if (!Initialized)
            Initialize();
    }

    /* Public methods. */
    public abstract Element Copy();

    public void CopyFrom(Element element)
    {
        LocalIndentation = element.LocalIndentation;
    }

    /* Godot overrides. */
    public override void _EnterTree()
    {
        if (!Initialized)
            Initialize();
    }

    public override void _Process(double delta)
    {
        Indentation.CustomMinimumSize = new Vector2(GlobalIndentation, 0f);
        Indentation.Visible = Indentation.CustomMinimumSize.X != 0;
    }

    /* Protected methods. */
    protected virtual void Initialize()
    {
        Initialized = true;

        // Set horizontal size flags.
        SizeFlagsHorizontal = SizeFlags.ExpandFill;

        // Create spacer for indentation.
        Indentation = new();
        Indentation.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        AddChild(Indentation);
        Indentation.Name = "Indentation";
    }
}
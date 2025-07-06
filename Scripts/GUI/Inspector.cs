using Godot;
using Godot.Collections;

namespace Rusty.ISA.Editor;

public partial class Inspector : Element
{
    /* Public properties. */
    /// <summary>
    /// The title of this inspector.
    /// </summary>
    public string Title
    {
        get
        {
            if (Border is LabelBorderContainer labeled)
                return labeled.LabelText;
            if (Border is FoldoutBorderContainer foldout)
                return foldout.FoldoutText;
            if (Border is CheckBoxBorderContainer checkbox)
                return checkbox.CheckBoxText;
            if (Border is ListBorderContainer list)
                return list.FoldoutText;
            return "";
        }
        set
        {
            if (Border is LabelBorderContainer labeled)
                labeled.LabelText = value;
            if (Border is FoldoutBorderContainer foldout)
                foldout.FoldoutText = value;
            if (Border is CheckBoxBorderContainer checkbox)
                checkbox.CheckBoxText = value;
            if (Border is ListBorderContainer list)
                list.FoldoutText = value;
        }
    }
    /// <summary>
    /// The fields of this inspector.
    /// </summary>
    public Dictionary<string, Element> Elements { get; } = new();

    /* Private properties. */
    private BorderContainer Border { get; set; }

    /* Public methods. */
    public override Inspector Copy()
    {
        Inspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(Inspector other)
    {
        CopyFrom(other as Element);

        // Make sure we have the correct border type.
        if (other.Border is FoldoutBorderContainer foldout)
            MakeFoldout(foldout.FoldoutOpen);
        if (other.Border is CheckBoxBorderContainer checkbox)
            MakeCheckBox(checkbox.CheckBoxEnabled);

        // Copy title.
        Title = other.Title;

        // Copy contents.
        foreach (var element in other.Elements)
        {
            AddElement(element.Key, element.Value.Copy());
        }
    }

    public void AddElement(string id, Element element)
    {
        if (!Elements.ContainsKey(id))
        {
            Border.AddToContents(element);
            Elements.Add(id, element);
        }
    }

    public Control TryGetElement(string id)
    {
        if (Elements.TryGetValue(id, out var element))
            return element;
        return null;
    }

    public void MakeFoldout(bool open)
    {
        // Remove old border.
        string title = Title;
        RemoveChild(Border);

        // Add checkbox container.
        FoldoutBorderContainer border = new();
        Border = border;
        Border.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Border);
        Border.Name = "Border";

        // Set defaults.
        border.FoldoutText = title;
        border.FoldoutOpen = open;
    }

    public void MakeCheckBox(bool enabled)
    {
        // Remove old border.
        string title = Title;
        RemoveChild(Border);

        // Add checkbox container.
        CheckBoxBorderContainer border = new();
        Border = border;
        Border.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Border);
        Border.Name = "Border";

        // Set defaults.
        border.CheckBoxText = title;
        border.CheckBoxEnabled = enabled;
    }

    public void MakeDropdown(int selected)
    {
        // Remove old border.
        string title = Title;
        RemoveChild(Border);

        // Add checkbox container.
        DropdownBorderContainer border = new();
        Border = border;
        Border.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Border);
        Border.Name = "Border";

        // Set defaults.
        border.DropdownText = title;
        border.SelectedOption = selected;
    }

    public void MakeList(bool foldoutOpen, Element template, string addButtonText)
    {
        // Remove old border.
        string title = Title;
        RemoveChild(Border);

        // Add checkbox container.
        ListBorderContainer border = new();
        Border = border;
        Border.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Border);
        Border.Name = "Border";

        // Set defaults.
        border.FoldoutText = title;
        border.FoldoutOpen = foldoutOpen;
        border.Template = template;
        border.AddButtonText = addButtonText;
    }

    public void HideBorder()
    {
        Border.HideHeader = true;
        Border.HideFooter = true;
        Border.ForceHideBorder = true;
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add border container.
        Border = new LabelBorderContainer();
        Border.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Border);
        Border.Name = "Border";
    }
}
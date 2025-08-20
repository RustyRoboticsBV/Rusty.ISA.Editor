namespace Rusty.ISA.Editor;

public partial class ListRuleInspector : RuleInspector
{
    /* Public properties. */
    public new ListRule Rule => base.Rule as ListRule;

    public new ListRulePreviewInstance Preview
    {
        get => base.Preview as ListRulePreviewInstance;
        protected set => base.Preview = value;
    }

    /* Constructors. */
    public ListRuleInspector() : base()
    {
        // Create list.
        ListBorderContainer list = new();
        ReplaceContainer(list);
    }

    public ListRuleInspector(InstructionSet set, ListRule rule)
        : base(set, rule)
    {
        // Create element template.
        RuleInspector template = RuleInspectorFactory.Create(set, rule.Type);
        FoldoutBorderContainer templateFoldout = new();
        templateFoldout.FoldoutText = rule.Type.DisplayName;
        templateFoldout.IsOpen = true;
        template.ReplaceContainer(templateFoldout);

        // Create list.
        ListBorderContainer list = new();
        list.FoldoutText = rule.DisplayName;
        list.AddButtonText = rule.AddButtonText;
        list.Template = template;
        list.Template.Name = "Template";
        ReplaceContainer(list);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        ListRuleInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void AddElement()
    {
        (ContentsContainer as ListBorderContainer).Add();
    }

    public int GetElementCount()
    {
        return ContentsContainer.GetContentsCount();
    }

    public RuleInspector GetElementInspector(int index)
    {
        return ContentsContainer.GetFromContents(index) as RuleInspector;
    }

    /* Protected methods. */
    protected override void UpdatePreview()
    {
        for (int i = 0; i < GetElementCount(); i++)
        {
            RuleInspector inspector = GetElementInspector(i);
            Preview.SetElement(i, inspector.Preview);
        }
        Preview.SetCount(GetElementCount());
    }
}
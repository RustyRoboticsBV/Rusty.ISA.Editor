using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public static class CompileRuleInspectorFactory
{
    public static Inspector Create(InstructionSet set, CompileRule rule)
    {
        // Create inspector.
        Inspector inspector = new();

        switch (rule)
        {
            case InstructionRule i:
                {
                    InstructionDefinition definition = set[i.Opcode];
                    Inspector childInspector = InstructionInspectorFactory.Create(set, definition);
                    inspector.Add("instruction", childInspector);
                    break;
                }
            case OptionRule o:
                {
                    CheckBoxBorderContainer checkBox = new();
                    checkBox.CheckBoxText = o.DisplayName;
                    inspector.ReplaceContainer(checkBox);

                    Inspector childInspector = Create(set, o.Type);
                    inspector.Add("type", childInspector);
                    break;
                }
            case ChoiceRule c:
                {
                    DropdownBorderContainer dropdown = new();
                    dropdown.DropdownText = c.DisplayName;
                    inspector.ReplaceContainer(dropdown);

                    foreach (CompileRule type in c.Types)
                    {
                        Inspector childInspector = Create(set, type);
                        inspector.Add("type" + inspector.GetContentsCount(), childInspector);
                    }
                    break;
                }
            case TupleRule t:
                {
                    foreach (CompileRule type in t.Types)
                    {
                        Inspector childInspector = Create(set, type);
                        inspector.Add("type" + inspector.GetContentsCount(), childInspector);
                    }
                    break;
                }
            case ListRule l:
                {
                    Inspector template = Create(set, l.Type);
                    template.ReplaceContainer(new FoldoutBorderContainer() { FoldoutText = l.Type.DisplayName });

                    ListBorderContainer list = new();
                    list.FoldoutText = l.DisplayName;
                    list.AddButtonText = l.AddButtonText;
                    list.Template = template;
                    list.Template.Name = "Template";
                    inspector.ReplaceContainer(list);

                    break;
                }
        }

        return inspector;
    }
}
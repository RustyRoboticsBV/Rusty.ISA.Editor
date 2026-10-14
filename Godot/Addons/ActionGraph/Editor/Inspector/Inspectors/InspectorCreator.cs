using System;

namespace Rusty.ActionGraph.Editor;

internal static class InspectorCreator
{
    public static IInspector Create(EditorDefinition definition)
    {
        switch (definition)
        {
            case NodeDefinition node:
                return new NodeInspector(node);
            case FormDefinition form:
                return new FormInspector(form);
            case OptionDefinition option:
                return new OptionInspector(option);
            case ChoiceDefinition choice:
                return new ChoiceInspector(choice);
            case TupleDefinition tuple:
                return new TupleInspector(tuple);
            case ListDefinition list:
                return new ListInspector(list);
            default:
                throw new Exception("Unknown definition type.");
        }
    }
}
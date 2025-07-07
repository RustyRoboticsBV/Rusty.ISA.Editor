namespace Rusty.ISA.Editor;

public static class NodeInspectorFactory
{
    public static Inspector Create(InstructionSet set, InstructionDefinition definition)
    {
        // Create inspector.
        Inspector inspector = new();

        // Change contents container.
        FoldoutBorderContainer foldout = new();
        foldout.FoldoutText = definition.DisplayName;
        foldout.IsOpen = true;
        inspector.ReplaceContainer(foldout);

        // Add title.
        LabeledIcon title = new();
        title.LabelText = definition.DisplayName;
        title.Texture = definition.Icon;
        inspector.Add("title", title);

        // Add start point checkbox.
        ToggleTextField startPoint = new();
        startPoint.LabelText = "Start Point";
        startPoint.Value = "Start";
        inspector.Add("start_point", startPoint);

        // Create instruction inspector.
        Inspector instructionInspector = InstructionInspectorFactory.Create(set, definition);
        inspector.Add("instruction", instructionInspector);

        return inspector;
    }
}
using Godot;
using Rusty.ActionGraph.Editor;

[GlobalClass]
public partial class TestEditor : EditorWindow
{
    public TestEditor() : base()
    {
        AddNodeDefinition(new NodeDefinition("dialog", "Dialog", "A dialog.", null, "Dialog", Colors.Azure, []));
    }
}
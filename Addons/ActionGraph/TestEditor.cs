using Godot;
using Rusty.ActionGraph.Editor;

[GlobalClass]
public partial class TestEditor : EditorWindow
{
    public TestEditor() : base()
    {
        Graph.CustomMinimumSize = new(0, 400);
        AddNodeDefinition(new NodeDefinition("dialog", "Dialog", "A dialog.", null, "Dialog", Colors.Azure, []));
    }
}
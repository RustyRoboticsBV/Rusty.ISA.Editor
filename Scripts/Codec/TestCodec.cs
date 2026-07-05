using Godot;

namespace Rusty.ISA.Serialization;

[GlobalClass]
public partial class TestCodec : Node
{
    public override void _EnterTree()
    {
        FileAccess file = FileAccess.Open("res://Test.txt", FileAccess.ModeFlags.Read);
        string text = file.GetAsText();
        FileNode node = XmlLoader.Load(text);
        GD.Print(node.Serialize());
        node.ToInstructions();
    }
}
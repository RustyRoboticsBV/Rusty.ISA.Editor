using Godot;

namespace Rusty.ActionGraph.Editor;

internal interface IField
{
    public string TitleText { get; set; }
    public int TitleWidth { get; set; }
    public string TooltipText { get; set; }
    public UndoRedo UndoRedo { get; set; }
}

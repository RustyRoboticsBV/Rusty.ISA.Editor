using Godot;

namespace Rusty.ISA.Editor;

public partial class LocalizedField : VBoxContainer
{
    public IField Field { get; private set; }

    public LocalizedField(IField field)
    {
        Field = field;
    }


}
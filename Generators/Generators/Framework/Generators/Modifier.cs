public enum ModifierType { None, Abstract, Virtual, Override, SealedOverride, Static, New, NewVirtual, NewAbstract, NewStatic };

/// <summary>
/// A modifier keyword generator.
/// </summary>
public sealed class Modifier : Generator
{
    public ModifierType Type { get; set; } = ModifierType.None;

    public static implicit operator Modifier(ModifierType type) => new() { Type = type };

    public override bool Generate(CodeWriter w)
    {
        switch (Type)
        {
            case ModifierType.Abstract:
                w.Append("abstract");
                return true;
            case ModifierType.Virtual:
                w.Append("virtual");
                return true;
            case ModifierType.Override:
                w.Append("override");
                return true;
            case ModifierType.SealedOverride:
                w.Append("sealed override");
                return true;
            case ModifierType.Static:
                w.Append("static");
                return true;
            case ModifierType.NewAbstract:
                w.Append("new abstract");
                return true;
            case ModifierType.NewVirtual:
                w.Append("new virtual");
                return true;
            case ModifierType.NewStatic:
                w.Append("new static");
                return true;
            default:
                return false;
        }
    }
}
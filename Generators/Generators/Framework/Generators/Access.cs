public enum AccessType { None, Public, Protected, Private, Internal, ProtectedInternal, PrivateProtected };

/// <summary>
/// An access keyword generator.
/// </summary>
public sealed class Access : Generator
{
    public AccessType Type { get; set; } = AccessType.Public;

    public static implicit operator Access(AccessType type) => new() { Type = type };

    public override bool Generate(CodeWriter w)
    {
        switch (Type)
        {
            case AccessType.Public:
                w.Append("public");
                return true;
            case AccessType.Protected:
                w.Append("protected");
                return true;
            case AccessType.Private:
                w.Append("private");
                return true;
            case AccessType.Internal:
                w.Append("internal");
                return true;
            case AccessType.ProtectedInternal:
                w.Append("protected internal");
                return true;
            case AccessType.PrivateProtected:
                w.Append("private protected");
                return true;
            default:
                return false;
        }
    }
}
namespace ParadeDB.EFCore.BM25Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class BM25IndexAttribute : Attribute
{
    public string? SearchTokenizer { get; init; }
}

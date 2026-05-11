using ParadeDB.EFCore.BM25Attributes;

namespace ParadeDB.EFCore.IntegrationTests.Persistence.Entities;

[BM25Index]
[BM25DefaultTokenizer]
public sealed class Item
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [BM25SimpleTokenizer(AliasSuffix = "simple")]
    public string Description { get; set; } = string.Empty;
}

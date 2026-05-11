using ParadeDB.EFCore.BM25Attributes;

namespace ParadeDB.EFCore.IntegrationTests.Persistence.Entities;

[BM25Index]
[BM25DefaultTokenizer]
public sealed class Product
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProductDetails Details { get; set; } = new();
}

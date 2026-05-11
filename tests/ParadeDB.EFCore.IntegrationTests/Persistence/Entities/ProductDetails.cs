using ParadeDB.EFCore.BM25Attributes;

namespace ParadeDB.EFCore.IntegrationTests.Persistence.Entities;

public sealed class ProductDetails
{
    [BM25LiteralTokenizer]
    [BM25IcuTokenizer(AliasSuffix = "icu")]
    public string Summary { get; set; } = string.Empty;

    [BM25IcuTokenizer]
    public string[] Tags { get; set; } = [];
}

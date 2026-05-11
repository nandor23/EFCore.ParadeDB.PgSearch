using ParadeDB.EFCore.BM25Attributes;

namespace ParadeDB.EFCore.Tests.TestModels;

public sealed class ProductDetails
{
    [BM25LiteralTokenizer]
    public string Summary { get; set; } = string.Empty;

    [BM25IcuTokenizer]
    public string[] Tags { get; set; } = [];
}

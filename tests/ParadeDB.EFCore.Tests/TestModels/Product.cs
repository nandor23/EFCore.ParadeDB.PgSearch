using System.Diagnostics.CodeAnalysis;
using ParadeDB.EFCore.BM25Attributes;

namespace ParadeDB.EFCore.Tests.TestModels;

[ExcludeFromCodeCoverage]
[BM25Index]
public sealed class Product
{
    public long Id { get; set; }

    [BM25DefaultTokenizer]
    public string Name { get; set; } = string.Empty;

    [BM25LiteralTokenizer]
    [BM25IcuTokenizer(AliasSuffix = "icu")]
    public string Description { get; set; } = string.Empty;

    public ProductDetails? Details { get; set; }
}

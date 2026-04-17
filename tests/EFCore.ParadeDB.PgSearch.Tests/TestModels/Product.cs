using System.Diagnostics.CodeAnalysis;

namespace EFCore.ParadeDB.PgSearch.Tests.TestModels;

[ExcludeFromCodeCoverage]
public sealed class Product
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

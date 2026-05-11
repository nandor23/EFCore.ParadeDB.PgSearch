using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ParadeDB.EFCore.Tests.TestModels;
using ParadeDB.EFCore.Tests.TestUtils;
using Shouldly;

namespace ParadeDB.EFCore.Tests.Metadata;

public sealed class BM25ConventionTests
{
    private static (IReadOnlyIndex Index, List<string> AnnotationNames) GetBm25IndexInfo(IModel model)
    {
        var entityType = model.FindEntityType(typeof(Product))!;
        var index = entityType.GetIndexes().ToList()
            .First(i => i.Properties.Any(p => p.Name == "Description"));
        var names = index.GetAnnotations().Select(a => a.Name).ToList();
        return (index, names);
    }

    [Test]
    public void Entity_HasBm25Index()
    {
        using var context = new TestDbContext();
        var (_, annotationNames) = GetBm25IndexInfo(context.Model);

        annotationNames.ShouldContain("ParadeDb:ColumnTokenizers",
            $"Annotations found: {string.Join(", ", annotationNames)}");
    }

    [Test]
    public void IndexableProperty_IsIncludedInIndex()
    {
        using var context = new TestDbContext();
        var (index, _) = GetBm25IndexInfo(context.Model);

        index.Properties.Any(p => p.Name == "Name").ShouldBeTrue();
    }

    [Test]
    public void PropertyWithLiteralTokenizer_GeneratesLiteralTokenization()
    {
        using var context = new TestDbContext();
        var (index, _) = GetBm25IndexInfo(context.Model);

        var columnTokenizers = index.GetAnnotation("ParadeDb:ColumnTokenizers")?.Value as string[];
        columnTokenizers.ShouldNotBeNull();
        columnTokenizers!.ShouldContain(t => t != null && t.Contains("pdb.literal"));
    }

    [Test]
    public void PropertyWithMultipleTokenizers_GeneratesAdditionalColumns()
    {
        using var context = new TestDbContext();
        var (index, _) = GetBm25IndexInfo(context.Model);

        var additionalColumns = index.GetAnnotation("ParadeDb:AdditionalIndexColumns")?.Value as string[];
        additionalColumns.ShouldNotBeNull();

        var additionalTokenizers = index.GetAnnotation("ParadeDb:AdditionalColumnTokenizers")?.Value as string[];
        additionalTokenizers.ShouldNotBeNull();

        additionalColumns!.ShouldContain("description");
        additionalTokenizers!.Any(t => t != null && t.Contains("pdb.icu")).ShouldBeTrue();
    }

    [Test]
    public void OwnedEntity_WithTokenizer_GeneratesIndexExpression()
    {
        using var context = new TestDbContext();
        var (index, _) = GetBm25IndexInfo(context.Model);

        var additionalExpressions = index.GetAnnotation("ParadeDb:AdditionalIndexExpressions")?.Value as string[];
        additionalExpressions.ShouldNotBeNull();
        additionalExpressions.ShouldNotBeEmpty();
    }
}

using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class AliasTranslatorTests
{
    [Test]
    public void Alias_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p => EF.Functions.Alias(p.Description, "description_simple"))
            .ToQueryString();

        sql.ShouldContain("p.description::pdb.alias('description_simple')");
    }
}

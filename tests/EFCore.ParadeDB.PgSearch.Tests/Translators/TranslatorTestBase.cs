using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public abstract class TranslatorTestBase
{
    protected static DbContextOptions<TestDbContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(
                "Host=localhost;Database=fake;Username=fake;Password=fake",
                o => o.UsePgSearch()
            )
            .Options;
    }
}

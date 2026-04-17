using EFCore.ParadeDB.PgSearch.IntegrationTests.Persistence;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public class TestBase
{
    [ClassDataSource<DbFixture>(Shared = SharedType.PerTestSession)]
    public required DbFixture DbFixture { get; init; }
}

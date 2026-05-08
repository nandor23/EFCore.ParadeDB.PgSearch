using ParadeDB.EFCore.IntegrationTests.Persistence;

namespace ParadeDB.EFCore.IntegrationTests;

public class TestBase
{
    [ClassDataSource<DbFixture>(Shared = SharedType.PerTestSession)]
    public required DbFixture DbFixture { get; init; }
}

using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests;

public sealed class BoostTests
{
    [Test]
    public void With_SetsFactorCorrectly()
    {
        var boost = Boost.With(2.5f);

        boost.Factor.ShouldBe(2.5f);
    }

    [Test]
    public void ToString_ReturnsCorrectFormat()
    {
        var boost = Boost.With(2.5f);

        boost.ToString().ShouldBe("boost(2.5)");
    }
}

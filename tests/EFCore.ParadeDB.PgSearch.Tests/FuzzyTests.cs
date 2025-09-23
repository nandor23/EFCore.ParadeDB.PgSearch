using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests;

public sealed class FuzzyTests
{
    [Test]
    public void With_Distance_SetsValuesCorrectly()
    {
        var fuzzy = Fuzzy.With(2);

        fuzzy.Distance.ShouldBe(2);
        fuzzy.Prefix.ShouldBe(false);
        fuzzy.TranspositionCostOne.ShouldBe(false);
    }

    [Test]
    public void With_DistanceAndPrefix_SetsValuesCorrectly()
    {
        var fuzzy = Fuzzy.With(3, true);

        fuzzy.Distance.ShouldBe(3);
        fuzzy.Prefix.ShouldBe(true);
        fuzzy.TranspositionCostOne.ShouldBe(false);
    }

    [Test]
    public void With_AllParameters_SetsValuesCorrectly()
    {
        var fuzzy = Fuzzy.With(1, true, true);

        fuzzy.Distance.ShouldBe(1);
        fuzzy.Prefix.ShouldBe(true);
        fuzzy.TranspositionCostOne.ShouldBe(true);
    }

    [Test]
    public void ToString_DistanceOnly_ReturnsCorrectFormat()
    {
        var fuzzy = Fuzzy.With(2);

        fuzzy.ToString().ShouldBe("fuzzy(2)");
    }

    [Test]
    public void ToString_WithPrefix_ReturnsCorrectFormat()
    {
        var fuzzy = Fuzzy.With(2, true);

        fuzzy.ToString().ShouldBe("fuzzy(2, t)");
    }

    [Test]
    public void ToString_WithTranspositionCostOne_ReturnsCorrectFormat()
    {
        var fuzzy = Fuzzy.With(2, false, true);

        fuzzy.ToString().ShouldBe("fuzzy(2, f, t)");
    }

    [Test]
    public void ToString_WithBothBooleans_ReturnsCorrectFormat()
    {
        var fuzzy = Fuzzy.With(2, true, true);

        fuzzy.ToString().ShouldBe("fuzzy(2, t, t)");
    }
}

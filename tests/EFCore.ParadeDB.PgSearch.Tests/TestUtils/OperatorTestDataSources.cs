namespace EFCore.ParadeDB.PgSearch.Tests.TestUtils;

public static class OperatorTestDataSources
{
    public static IEnumerable<Func<(Fuzzy, Boost)>> FuzzyBoostTestData()
    {
        yield return () => (Fuzzy.With(2), Boost.With(3));
        yield return () => (Fuzzy.With(3, false), Boost.With(2));
        yield return () => (Fuzzy.With(3, true), Boost.With(2));
        yield return () => (Fuzzy.With(3, false, false), Boost.With(2));
        yield return () => (Fuzzy.With(3, true, false), Boost.With(2));
        yield return () => (Fuzzy.With(3, false, true), Boost.With(2));
        yield return () => (Fuzzy.With(3, true, true), Boost.With(2));
    }
}

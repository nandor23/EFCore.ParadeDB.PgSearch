namespace EFCore.ParadeDB.PgSearch.Tests.TestUtils;

public static class OperatorTestDataSources
{
    public static IEnumerable<Func<(Fuzzy, Boost)>> FuzzyBoostTestData()
    {
        yield return () => (Fuzzy.With(0), Boost.With(3));
        yield return () => (Fuzzy.With(0, false), Boost.With(2.3f));
        yield return () => (Fuzzy.With(1, true), Boost.With(2.3f));
        yield return () => (Fuzzy.With(1, false, false), Boost.With(2.5f));
        yield return () => (Fuzzy.With(2, true, false), Boost.With(2.5f));
        yield return () => (Fuzzy.With(2, false, true), Boost.With(2.5f));
        yield return () => (Fuzzy.With(2, true, true), Boost.With(2.5f));
    }
}

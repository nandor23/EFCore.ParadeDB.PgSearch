using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Expressions;

internal sealed class FuzzyTypeMapping : RelationalTypeMapping
{
    private static readonly Type FuzzyType = typeof(Fuzzy);

    public FuzzyTypeMapping(Fuzzy fuzzy)
        : base(fuzzy.ToString(), FuzzyType) { }

    private FuzzyTypeMapping(RelationalTypeMappingParameters parameters)
        : base(parameters) { }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
    {
        return new FuzzyTypeMapping(parameters);
    }
}

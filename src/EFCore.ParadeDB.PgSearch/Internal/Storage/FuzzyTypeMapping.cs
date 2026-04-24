using EFCore.ParadeDB.PgSearch.Modifiers;

using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internal.Storage;

internal sealed class FuzzyTypeMapping : RelationalTypeMapping
{
    public FuzzyTypeMapping(Fuzzy fuzzy)
        : base(fuzzy.ToString(), typeof(Fuzzy)) { }

    private FuzzyTypeMapping(RelationalTypeMappingParameters parameters)
        : base(parameters) { }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
    {
        return new FuzzyTypeMapping(parameters);
    }
}

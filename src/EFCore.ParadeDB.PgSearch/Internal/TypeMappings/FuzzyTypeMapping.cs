using EFCore.ParadeDB.PgSearch.Internal.Modifiers;

using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internal.TypeMappings;

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

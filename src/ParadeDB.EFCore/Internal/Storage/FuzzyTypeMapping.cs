using Microsoft.EntityFrameworkCore.Storage;
using ParadeDB.EFCore.Modifiers;

namespace ParadeDB.EFCore.Internal.Storage;

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

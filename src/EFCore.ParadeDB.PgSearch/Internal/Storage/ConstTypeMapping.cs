using EFCore.ParadeDB.PgSearch.Modifiers;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internal.Storage;

internal sealed class ConstTypeMapping : RelationalTypeMapping
{
    public ConstTypeMapping(Const @const)
        : base(@const.ToString(), typeof(Const)) { }

    private ConstTypeMapping(RelationalTypeMappingParameters parameters)
        : base(parameters) { }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
    {
        return new ConstTypeMapping(parameters);
    }
}

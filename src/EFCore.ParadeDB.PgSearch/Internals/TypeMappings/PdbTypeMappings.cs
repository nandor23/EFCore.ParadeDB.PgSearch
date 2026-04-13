using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internals.TypeMappings;

internal static class PdbTypeMappings
{
    public static readonly RelationalTypeMapping Boolean = new BoolTypeMapping("boolean");
}

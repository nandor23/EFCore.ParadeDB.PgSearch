using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internal.Storage;

internal static class PdbTypeMappings
{
    public static readonly RelationalTypeMapping Boolean = new BoolTypeMapping("boolean");
}

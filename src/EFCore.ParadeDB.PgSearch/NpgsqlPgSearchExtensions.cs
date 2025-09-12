using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

// ReSharper disable SuspiciousTypeConversion.Global

namespace EFCore.ParadeDB.PgSearch;

public static class NpgsqlPgSearchExtensions
{
    public static DbContextOptionsBuilder UsePgSearch(this DbContextOptionsBuilder optionsBuilder)
    {
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(
            new PgSearchOptionsExtension()
        );
        return optionsBuilder;
    }
}

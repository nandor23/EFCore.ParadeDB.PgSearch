using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

// ReSharper disable SuspiciousTypeConversion.Global

namespace EFCore.ParadeDB.PgSearch;

public static class NpgsqlPgSearchExtensions
{
    public static NpgsqlDbContextOptionsBuilder UsePgSearch(
        this NpgsqlDbContextOptionsBuilder builder
    )
    {
        ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(
            new PgSearchOptionsExtension()
        );
        return builder;
    }
}

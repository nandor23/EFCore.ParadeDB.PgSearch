using EFCore.ParadeDB.PgSearch.Internals;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

// ReSharper disable SuspiciousTypeConversion.Global

namespace EFCore.ParadeDB.PgSearch;

public static class NpgsqlPgSearchExtensions
{
    public static NpgsqlDbContextOptionsBuilder UsePgSearch(
        this NpgsqlDbContextOptionsBuilder optionsBuilder
    )
    {
        var coreOptionsBuilder = (
            (IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder
        ).OptionsBuilder;

        ((IDbContextOptionsBuilderInfrastructure)coreOptionsBuilder).AddOrUpdateExtension(
            new PgSearchOptionsExtension()
        );

        return optionsBuilder;
    }
}

using EFCore.ParadeDB.PgSearch.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

// ReSharper disable SuspiciousTypeConversion.Global

namespace EFCore.ParadeDB.PgSearch.Extensions;

public static class PgSearchDbContextOptionsBuilderExtensions
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

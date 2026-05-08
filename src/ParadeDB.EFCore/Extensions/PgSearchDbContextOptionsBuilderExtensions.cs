using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using ParadeDB.EFCore.Internal;

// ReSharper disable SuspiciousTypeConversion.Global

namespace ParadeDB.EFCore.Extensions;

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

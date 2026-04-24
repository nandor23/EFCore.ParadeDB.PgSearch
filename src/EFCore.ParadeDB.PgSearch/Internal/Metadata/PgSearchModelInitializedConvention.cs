using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace EFCore.ParadeDB.PgSearch.Internal.Metadata;

internal sealed class PgSearchModelInitializedConvention : IModelInitializedConvention
{
    public void ProcessModelInitialized(
        IConventionModelBuilder modelBuilder,
        IConventionContext<IConventionModelBuilder> context
    )
    {
        modelBuilder.HasPostgresExtension("pg_search");
    }
}

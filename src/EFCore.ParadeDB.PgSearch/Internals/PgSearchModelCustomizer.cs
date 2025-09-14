using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFCore.ParadeDB.PgSearch.Internals;

// TODO: remove if not needed
internal sealed class PgSearchModelCustomizer : ModelCustomizer
{
    public PgSearchModelCustomizer(ModelCustomizerDependencies dependencies)
        : base(dependencies) { }

    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        base.Customize(modelBuilder, context);

        // For non-generic methods
        // modelBuilder.HasDbFunction(typeof(PgSearch).GetMethod(nameof(PgSearch.Score)));
    }
}

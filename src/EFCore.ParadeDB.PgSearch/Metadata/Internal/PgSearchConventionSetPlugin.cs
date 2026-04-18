using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace EFCore.ParadeDB.PgSearch.Metadata.Internal;

internal sealed class PgSearchConventionSetPlugin : IConventionSetPlugin
{
    public ConventionSet ModifyConventions(ConventionSet conventionSet)
    {
        conventionSet.ModelInitializedConventions.Add(new PgSearchModelInitializedConvention());
        return conventionSet;
    }
}

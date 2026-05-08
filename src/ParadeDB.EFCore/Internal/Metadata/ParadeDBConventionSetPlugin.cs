using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace ParadeDB.EFCore.Internal.Metadata;

internal sealed class ParadeDbConventionSetPlugin : IConventionSetPlugin
{
    public ConventionSet ModifyConventions(ConventionSet conventionSet)
    {
        conventionSet.ModelInitializedConventions.Add(new ParadeDbModelInitializedConvention());
        return conventionSet;
    }
}

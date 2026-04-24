using EFCore.ParadeDB.PgSearch.Internal.Metadata;
using EFCore.ParadeDB.PgSearch.Internal.Query;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ParadeDB.PgSearch.Internal;

internal sealed class PgSearchOptionsExtension : IDbContextOptionsExtension
{
    public PgSearchOptionsExtension()
    {
        Info = new PgSearchOptionsExtensionInfo(this);
    }

    public void ApplyServices(IServiceCollection services)
    {
        services.AddScoped<IMethodCallTranslatorPlugin, PgSearchMethodCallTranslatorPlugin>();
        services.AddScoped<IMemberTranslatorPlugin, PgSearchMemberTranslatorPlugin>();
        services.AddSingleton<IConventionSetPlugin, PgSearchConventionSetPlugin>();
    }

    public void Validate(IDbContextOptions options) { }

    public DbContextOptionsExtensionInfo Info { get; }

    private sealed class PgSearchOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public PgSearchOptionsExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension) { }

        public override int GetServiceProviderHashCode() => 0;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) =>
            true;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            debugInfo["PgSearch"] = "Enabled";
        }

        public override bool IsDatabaseProvider => false;
        public override string LogFragment => "using PgSearch ";
    }
}

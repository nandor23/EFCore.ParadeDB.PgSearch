using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ParadeDB.PgSearch.Internals;

internal sealed class PgSearchOptionsExtension : IDbContextOptionsExtension
{
    public PgSearchOptionsExtension()
    {
        Info = new PgSearchOptionsExtensionInfo(this);
    }

    public void ApplyServices(IServiceCollection services)
    {
        services.AddScoped<IMethodCallTranslatorPlugin, PgSearchTranslatorPlugin>();
    }

    public void Validate(IDbContextOptions options)
    {
        // TODO
        // throw new NotImplementedException();
    }

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

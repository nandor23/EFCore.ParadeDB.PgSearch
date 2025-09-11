using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.PgSearch;

internal sealed class PgSearchOptionsExtension : IDbContextOptionsExtension
{
    public void ApplyServices(IServiceCollection services)
    {
        services.AddSingleton<IMethodCallTranslatorPlugin, PgSearchTranslatorPlugin>();
    }

    public void Validate(IDbContextOptions options)
    {
        // TODO
        throw new NotImplementedException();
    }

    public DbContextOptionsExtensionInfo Info { get; }
}

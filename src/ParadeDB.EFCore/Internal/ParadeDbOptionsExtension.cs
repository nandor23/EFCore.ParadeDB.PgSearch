using ParadeDB.EFCore.Internal.Metadata;
using ParadeDB.EFCore.Internal.Migrations;
using ParadeDB.EFCore.Internal.Query;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace ParadeDB.EFCore.Internal;

internal sealed class ParadeDbOptionsExtension : IDbContextOptionsExtension
{
    public ParadeDbOptionsExtension()
    {
        Info = new ParadeDbOptionsExtensionInfo(this);
    }

    public void ApplyServices(IServiceCollection services)
    {
        services.AddScoped<IMethodCallTranslatorPlugin, PgSearchMethodCallTranslatorPlugin>();
        services.AddScoped<IMemberTranslatorPlugin, ParadeDbMemberTranslatorPlugin>();
        services.AddSingleton<IConventionSetPlugin, ParadeDbConventionSetPlugin>();

        var migrationsGeneratorDescriptor = services.FirstOrDefault(
            descriptor => descriptor.ServiceType == typeof(IMigrationsSqlGenerator));

        if (migrationsGeneratorDescriptor is not null)
        {
            services.Remove(migrationsGeneratorDescriptor);
            services.Add(new ServiceDescriptor(typeof(IMigrationsSqlGenerator), sp =>
            {
                var generator = migrationsGeneratorDescriptor.ImplementationFactory is not null
                    ? (IMigrationsSqlGenerator)migrationsGeneratorDescriptor.ImplementationFactory(sp)
                    : migrationsGeneratorDescriptor.ImplementationInstance is not null
                        ? (IMigrationsSqlGenerator)migrationsGeneratorDescriptor.ImplementationInstance
                        : (IMigrationsSqlGenerator)ActivatorUtilities.CreateInstance(
                            sp,
                            migrationsGeneratorDescriptor.ImplementationType!);

                var dependencies = sp.GetRequiredService<MigrationsSqlGeneratorDependencies>();
                return new PgSearchMigrationsSqlGeneratorDecorator(generator, dependencies);
            }, migrationsGeneratorDescriptor.Lifetime));
        }

        var annotationProviderDescriptor = services.FirstOrDefault(
            descriptor => descriptor.ServiceType == typeof(IRelationalAnnotationProvider));

        if (annotationProviderDescriptor is not null)
        {
            services.Remove(annotationProviderDescriptor);
            services.Add(new ServiceDescriptor(typeof(IRelationalAnnotationProvider), sp =>
            {
                var provider = annotationProviderDescriptor.ImplementationFactory is not null
                    ? (IRelationalAnnotationProvider)annotationProviderDescriptor.ImplementationFactory(sp)
                    : annotationProviderDescriptor.ImplementationInstance is not null
                        ? (IRelationalAnnotationProvider)annotationProviderDescriptor.ImplementationInstance
                        : (IRelationalAnnotationProvider)ActivatorUtilities.CreateInstance(
                            sp,
                            annotationProviderDescriptor.ImplementationType!);

                return new ParadeDbRelationalAnnotationProvider(provider);
            }, annotationProviderDescriptor.Lifetime));
        }
    }

    public void Validate(IDbContextOptions options) { }

    public DbContextOptionsExtensionInfo Info { get; }

    private sealed class ParadeDbOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public ParadeDbOptionsExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension) { }

        public override int GetServiceProviderHashCode() => 0;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) =>
            true;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            debugInfo["ParadeDb"] = "Enabled";
        }

        public override bool IsDatabaseProvider => false;
        public override string LogFragment => "using ParadeDb ";
    }
}

using EFCore.ParadeDB.PgSearch.IntegrationTests.TestModels;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using TUnit.Core.Interfaces;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests.TestUtils;

public sealed class DbFixture : IAsyncInitializer, IAsyncDisposable
{
    private PostgreSqlContainer? _container;

    private DbContextOptions<TestDbContext> _options = null!;

    public async Task InitializeAsync()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("paradedb/paradedb:latest-pg14")
            .WithDatabase("pg_search_test")
            .WithUsername("test")
            .WithPassword("Pass!w0rd1")
            .Build();

        await _container.StartAsync();

        _options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_container.GetConnectionString(), o => o.UsePgSearch())
            .UseSnakeCaseNamingConvention()
            .Options;

        await using var context = new TestDbContext(_options);
        await context.Database.MigrateAsync();
        await SeedDefaultDataAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_container is not null)
        {
            await _container.DisposeAsync();
        }
    }

    public TestDbContext CreateContext() => new(_options);

    private async Task SeedDefaultDataAsync()
    {
        await using var context = CreateContext();

        var products = new[]
        {
            new Product
            {
                Name = "UltraComfort Memory Foam Running Shoes",
                Description = """
                    Experience unparalleled comfort with the UltraComfort Memory Foam Running Shoes.
                    Designed for athletes and casual joggers alike, these shoes feature a breathable mesh upper,
                    a cushioned memory foam insole that molds to your foot's unique shape, and a durable rubber outsole.
                    Whether you're hitting the pavement or enjoying a leisurely walk, these shoes provide the support
                    and comfort you need.
                    """,
            },
            new Product
            {
                Name = "ProStride Jogging Sneakers",
                Description = """
                    Elevate your running experience with the ProStride Jogging Sneakers.
                    Crafted with a lightweight, breathable mesh upper, these sneakers offer optimal ventilation
                    to keep your feet cool. The responsive cushioning system absorbs impact, reducing strain on your joints,
                    while the slip-resistant rubber outsole ensures stability on various surfaces.
                    Perfect for both seasoned runners and beginners.
                    """,
            },
            new Product
            {
                Name = "NoiseCancel Wireless Headphones",
                Description = """
                    Immerse yourself in high-quality sound with the NoiseCancel Wireless Headphones.
                    Featuring advanced noise-cancelling technology, these headphones block out ambient noise,
                    allowing you to focus on your music or calls. The ergonomic design ensures a comfortable fit
                    for extended wear, and the long-lasting battery provides hours of uninterrupted listening.
                    Ideal for travel, work, or leisure.
                    """,
            },
            new Product
            {
                Name = "StudioSound Over-Ear Headphones",
                Description = """
                    Discover exceptional audio clarity with the StudioSound Over-Ear Headphones.
                    Equipped with premium drivers, these headphones deliver rich bass and crisp highs,
                    providing an immersive listening experience. The plush ear cups and adjustable headband
                    offer a personalized fit, while the foldable design makes them easy to store and transport.
                    Suitable for audiophiles and casual listeners alike.
                    """,
            },
            new Product
            {
                Name = "EcoBrew French Press Coffee Maker",
                Description = """
                    Brew your favorite coffee with the EcoBrew French Press Coffee Maker.
                    Made from high-quality borosilicate glass and stainless steel, this French press ensures durability
                    and heat retention. The fine mesh filter allows essential oils and fine particles to pass through,
                    delivering a rich and full-bodied cup of coffee. Its sleek design and easy-to-use mechanism
                    make it a must-have for coffee enthusiasts.
                    """,
            },
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }
}

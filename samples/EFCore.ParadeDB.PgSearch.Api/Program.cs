using EFCore.ParadeDB.PgSearch;
using EFCore.ParadeDB.PgSearch.Api.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("AppDatabase"), o => o.UsePgSearch());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();

    var products = await dbContext
        .Products.Where(p =>
            EF.Functions.MatchDisjunction(
                p.Description,
                "with shoes and",
                Fuzzy.With(1),
                Boost.With(2.3f)
            )
        )
        .Select(p => new
        {
            p.Id,
            p.Description,
            Score = EF.Functions.Score(p.Id),
        })
        .ToListAsync();

    Console.WriteLine(products);
}

app.Run();

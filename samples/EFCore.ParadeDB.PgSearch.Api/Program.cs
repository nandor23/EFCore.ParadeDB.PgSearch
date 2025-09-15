using EFCore.ParadeDB.PgSearch;
using EFCore.ParadeDB.PgSearch.Api.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("Postgres"), o => o.UsePgSearch())
        .UseSnakeCaseNamingConvention();
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

    var products = dbContext
        .Products.Where(p =>
            EF.Functions.Term(p.Description, "with", Fuzzy.With(1), Boost.With(1))
            || EF.Functions.Term(p.Description, "shoes", Boost.With(1))
        )
        .Select(p => new
        {
            p.Id,
            p.Description,
            Score = EF.Functions.Score(p.Description),
        })
        .ToList();

    Console.WriteLine(products);
}

app.Run();

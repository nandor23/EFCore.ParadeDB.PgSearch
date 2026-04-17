using EFCore.ParadeDB.PgSearch;
using EFCore.ParadeDB.PgSearch.Api.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("AppDatabase"), o => o.UsePgSearch())
        .EnableSensitiveDataLogging()
        .UseSnakeCaseNamingConvention();
});

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();

    var value = "description_simple";
    var boost = Pdb.Boost(1);

    var result = dbContext
        .Products.Where(p => EF.Functions.Term(p.Description, new[] {"asd"}))
        .Select(p => EF.Functions.Score(p.Description))
        .ToList();

    Console.WriteLine(result);
}

app.Run();

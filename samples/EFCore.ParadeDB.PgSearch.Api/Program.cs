using EFCore.ParadeDB.PgSearch;
using EFCore.ParadeDB.PgSearch.Api.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("AppDatabase"),
        o => o.UsePgSearch()
    );
});

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();

    var products = dbContext
        .Products.Select(p => new
        {
            p.Id,
            Description = EF.Functions.Tokenize(
                p.Description,
                Tokenizer.Unicode(TokenFilter.AlphaNumericOnly)
            ),
        })
        .ToList();

    Console.WriteLine(products);
}

app.Run();

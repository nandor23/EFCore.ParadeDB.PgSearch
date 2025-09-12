using EFCore.ParadeDB.PgSearch;
using EFCore.ParadeDB.PgSearch.Api.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")).UsePgSearch();
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
    dbContext.Database.EnsureCreated();

    var products = dbContext.Products.ToList();
}

app.Run();

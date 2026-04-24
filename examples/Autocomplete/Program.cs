using Autocomplete;
using Autocomplete.Data;
using EFCore.ParadeDB.PgSearch.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

var connectionString = config.GetConnectionString("Default")!;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseNpgsql(connectionString, o => o.UsePgSearch())
    .UseSnakeCaseNamingConvention()
    .Options;

await using var db = new AppDbContext(options);

Console.WriteLine(new string('=', 60));
Console.WriteLine("EF ParadeDB Autocomplete Example");
Console.WriteLine("Fast as-you-type search");
Console.WriteLine(new string('=', 60));

await db.Database.EnsureDeletedAsync();
await db.Database.MigrateAsync();

var count = await db.AutocompleteItems.CountAsync();
Console.WriteLine($"Loaded {count} products from autocomplete_items table");

Console.WriteLine();
Console.WriteLine(new string('=', 60));
Console.WriteLine("Autocomplete");
Console.WriteLine(new string('=', 60));

string[] queries =
[
    "run",
    "runn",
    "running",
    "wire",
    "wirel",
    "wireles",
    "wireless",
    "blue",
    "blueto",
    "bluetooth",
];

foreach (var query in queries)
{
    Console.WriteLine($"\nUser types: '{query}' ->");

    var parseQuery = $"description_ngram:{query}";

    var results = db
        .AutocompleteItems
        .FromSqlInterpolated($"""
                              SELECT * FROM autocomplete_items
                              WHERE id @@@ pdb.parse({parseQuery})
                              """)
        .Select(x => new
        {
            x.Id,
            x.Description,
            x.Category,
            x.Rating,
            x.InStock,
            SearchScore = EF.Functions.Score(x.Id),
        })
        .OrderByDescending(x => x.SearchScore)
        .Take(5)
        .ToList();
    
    if (results.Count == 0)
    {
        Console.WriteLine("  (no results)");
        continue;
    }

    foreach (var item in results)
    {
        var desc = item.Description.Length > 50 ? item.Description[..47] + "..." : item.Description;

        Console.WriteLine($"  - {desc} (score: {item.SearchScore:F2})");
    }
}

Console.WriteLine("\nDone.");

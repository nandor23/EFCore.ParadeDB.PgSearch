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
    "run'",
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
    
    var results = await db.Database
        .SqlQuery<AutocompleteResult>($"""
                                       SELECT a.id, a.description, a.category, a.rating, a.in_stock,
                                              pdb.score(a.id) AS search_score
                                       FROM autocomplete_items AS a
                                       WHERE a.id @@@ pdb.parse({query}, lenient => true)
                                       ORDER BY pdb.score(a.id) DESC
                                       LIMIT 5
                                       """)
        .ToListAsync();

    if (results.Count == 0)
    {
        Console.WriteLine("  (no results)");
        continue;
    }

    foreach (var item in results)
    {
        var desc = item.Description.Length > 50 ? item.Description[..47] + "..." : item.Description;
        Console.WriteLine($"  [{item.Category}] {desc} (score: {item.SearchScore:F2})");
    }
}

Console.WriteLine("\nDone.");

public record AutocompleteResult(
    int Id,
    string Description,
    string Category,
    decimal Rating,
    bool InStock,
    double SearchScore
);

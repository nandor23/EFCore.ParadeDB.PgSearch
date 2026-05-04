using EFCore.ParadeDB.PgSearch.Extensions;
using HybridRrf.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var connectionString = config.GetConnectionString("Default");

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseNpgsql(connectionString, o => o.UsePgSearch())
    .UseSnakeCaseNamingConvention()
    .Options;

await using var dbContext = new AppDbContext(options);

await dbContext.Database.EnsureDeletedAsync();
await dbContext.Database.MigrateAsync();

Console.WriteLine(new string('=', 60));
Console.WriteLine("Hybrid Search with Reciprocal Rank Fusion (RRF)");
Console.WriteLine(new string('=', 60));

Console.WriteLine("Single-query CTE: BM25 (keyword) + Vector (semantic)");
Console.WriteLine("RRF formula: score = sum(1 / (k + rank)) across all rankings");

await LoadEmbeddingsAsync(dbContext);

return;

static async Task LoadEmbeddingsAsync(AppDbContext db)
{
    var csvPath = Path.Combine(AppContext.BaseDirectory, "mock_items_embeddings.csv");
    var lines = await File.ReadAllLinesAsync(csvPath);
    
    await using var conn = (NpgsqlConnection)db.Database.GetDbConnection();
    await conn.OpenAsync();

    var total = lines.Length - 1;
    for (var i = 1; i < lines.Length; i++)
    {
        var parts = lines[i].Split(',', 3);
        var id = int.Parse(parts[0]);
        var embedding = parts[2].Trim('"');

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE mock_items SET embedding = @embedding::vector WHERE id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@embedding", embedding);
        await cmd.ExecuteNonQueryAsync();
    }

    Console.WriteLine($"Loaded {total} embeddings");
}

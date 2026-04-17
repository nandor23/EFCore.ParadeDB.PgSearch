<h1 align="center">
    <img alt="Logo" src="logo/pgsearch-logo.svg" width="140"/>
    <br>
    EFCore.ParadeDB.PgSearch
</h1>

[![Build](https://github.com/nandor23/EFCore.ParadeDB.PgSearch/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/nandor23/EFCore.ParadeDB.PgSearch/actions/workflows/build.yml)
[![License](https://img.shields.io/github/license/nandor23/EFCore.ParadeDB.PgSearch)](https://opensource.org/license/mit)

EFCore.ParadeDB.PgSearch adds [ParadeDB](https://www.paradedb.com/)'s pg_search extension support to [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.npgsql.org/efcore/index.html?tabs=onconfiguring). 
It exposes ParadeDB search functions through **EF.Functions** API for LINQ-based full-text search queries. BM25 index creation must be done via raw SQL. See the [EF Core documentation](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing?tabs=dotnet-core-cli#adding-raw-sql) for more details on adding raw SQL to migrations.

## Configuration

Install the [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL/10.0.0-rc.1#readme-body-tab) NuGet package and configure your DbContext by calling `UsePgSearch()` on the `NpgsqlDbContextOptionsBuilder` to enable pg_search function mappings.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<AppDbContext>(opt =>
{
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("AppDatabase"),
        o => o.UsePgSearch()
    );
});
```

A complete example is also available in the [samples directory](https://github.com/nandor23/EFCore.ParadeDB.PgSearch/tree/main/samples/EFCore.ParadeDB.PgSearch.Api) demonstrating PgSearch configuration and BM25 index creation.

## Function Mappings

The following ParadeDB operations are available through the `EF.Functions` API:

| ParadeDB Operation                                                          | LINQ Methods                               |
|-----------------------------------------------------------------------------|--------------------------------------------|
| [Match](https://docs.paradedb.com/documentation/full-text/match)            | `MatchDisjunction()`, `MatchConjunction()` |
| [Phrase](https://docs.paradedb.com/documentation/full-text/phrase)          | `Phrase()`                                 |
| [Term](https://docs.paradedb.com/documentation/full-text/term)              | `Term()`                                   |
| [Highlighting](https://docs.paradedb.com/documentation/full-text/highlight) | `Snippet()`                                |
| [Proximity](https://docs.paradedb.com/documentation/full-text/proximity)    | `Proximity()`, `ProximityRegex()`          |
| [BM25 scoring](https://docs.paradedb.com/documentation/sorting/score)       | `Score()`                                  |

## Usage Example

```csharp
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
```

### Translates to:

```sql
SELECT p."Id", p."Description", COALESCE(pdb.score(p."Id"), 0) AS "Score"
FROM "Products" AS p
WHERE p."Description" ||| 'with shoes and'::pdb.fuzzy(1)::pdb.boost(2.3)
```
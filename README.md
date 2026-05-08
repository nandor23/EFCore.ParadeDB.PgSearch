<h1 align="center">
    <img alt="Logo" src="logo/pgsearch-logo.svg" width="150"/>
  <br>
  EFCore.ParadeDB.PgSearch
  <br>
</h1>

<h4 align="center">Entity Framework Core extension for <a href="https://www.paradedb.com" target="_blank">ParadeDB</a> pg_search</h4>

<p align="center">
  <a href="https://github.com/nandor23/EFCore.ParadeDB.PgSearch/actions/workflows/ci.yml"><img src="https://github.com/nandor23/EFCore.ParadeDB.PgSearch/actions/workflows/ci.yml/badge.svg?branch=main" alt="Build"></a>
  <img src="https://img.shields.io/nuget/dt/EFCore.ParadeDB.PgSearch?color=%235c6bc0" alt="NuGet Downloads">
  <a href="https://opensource.org/license/mit"><img src="https://img.shields.io/github/license/nandor23/EFCore.ParadeDB.PgSearch?color=%231e8e7e" alt="License"></a>
</p>

EFCore.ParadeDB.PgSearch adds support for ParadeDB's pg_search extension to [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.npgsql.org/efcore/index.html?tabs=onconfiguring), exposing ParadeDB search functions through the **EF.Functions** API for LINQ-based full-text search queries.
BM25 index creation must be defined using raw SQL. See the [EF Core documentation](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing?tabs=dotnet-core-cli#adding-raw-sql) for details on adding raw SQL to migrations.

## Requirements & Compatibility

| Component  | Supported                     |
|------------|-------------------------------|
| .NET       | 8, 9, 10                      |
| ParadeDB   | 0.23.0+                       |
| PostgreSQL | 15+ (with ParadeDB extension) |

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

## Function Mappings

The following ParadeDB operations are available through the `EF.Functions` API:

| ParadeDB Operation                                                          | LINQ Methods                               |
|-----------------------------------------------------------------------------|--------------------------------------------|
| [Match](https://docs.paradedb.com/documentation/full-text/match)            | `MatchDisjunction()`, `MatchConjunction()` |
| [Phrase](https://docs.paradedb.com/documentation/full-text/phrase)          | `Phrase()`                                 |
| [Term](https://docs.paradedb.com/documentation/full-text/term)              | `Term()`                                   |
| [Highlighting](https://docs.paradedb.com/documentation/full-text/highlight) | `Snippet()`                                |
| [Proximity](https://docs.paradedb.com/documentation/full-text/proximity)    | `Proximity()`                              |
| [BM25 scoring](https://docs.paradedb.com/documentation/sorting/score)       | `Score()`                                  |
| [Tokenizers](https://docs.paradedb.com/documentation/tokenizers/overview)   | `TokenizeAsArray()`                        |

## Examples

- [Quickstart](examples/Quickstart/Program.cs)
- [Faceted Search](examples/FacetedSearch/Program.cs)
- [Autocomplete](examples/Autocomplete/Program.cs)
- [More Like This](examples/MoreLikeThis/Program.cs)
- [Hybrid Search (RRF)](examples/HybridRrf/Program.cs)
- [RAG](examples/Rag/Program.cs)

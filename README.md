<h1 align="center">
    <img alt="Logo" src="assets/paradedb-logo.png" />
  <br>
  EFCore.ParadeDB.PgSearch
  <br>
</h1>

<h4 align="center">Entity Framework Core extension for <a href="https://www.paradedb.com" target="_blank">ParadeDB</a> pg_search</h4>

<p align="center">
  <a href="https://github.com/paradedb/efcore-paradedb/actions/workflows/ci.yml"><img src="https://github.com/paradedb/efcore-paradedb/actions/workflows/ci.yml/badge.svg?branch=main" alt="Build"></a>
  <img src="https://img.shields.io/nuget/dt/ParadeDB.EntityFrameworkCore?color=%235c6bc0" alt="NuGet Downloads">
  <a href="https://opensource.org/license/mit"><img src="https://img.shields.io/github/license/paradedb/efcore-paradedb?color=%231e8e7e" alt="License"></a>
</p>

EFCore.ParadeDB.PgSearch adds support for ParadeDB's pg_search extension to [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.npgsql.org/efcore/index.html?tabs=onconfiguring), exposing ParadeDB search functions through the **EF.Functions** API for LINQ-based full-text search queries.
BM25 index creation must be defined using raw SQL. See the [EF Core documentation](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing?tabs=dotnet-core-cli#adding-raw-sql) for details on adding raw SQL to migrations.

## Requirements & Compatibility

| Component  | Supported                     |
| ---------- | ----------------------------- |
| .NET       | 8, 9, 10                      |
| ParadeDB   | 0.23.0+                       |
| PostgreSQL | 15+ (with ParadeDB extension) |

## Configuration

Install the [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL/10.0.0-rc.1#readme-body-tab)
NuGet package and configure your DbContext by calling `UseParadeDb()` on the `NpgsqlDbContextOptionsBuilder` to enable pg_search function mappings.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<AppDbContext>(opt =>
{
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("AppDatabase"),
        o => o.UseParadeDb()
    );
});
```

## Function Mappings

The following ParadeDB operations are available through the `EF.Functions` API:

| ParadeDB Operation                                                          | LINQ Methods                               |
| --------------------------------------------------------------------------- | ------------------------------------------ |
| [Match](https://docs.paradedb.com/documentation/full-text/match)            | `MatchDisjunction()`, `MatchConjunction()` |
| [Phrase](https://docs.paradedb.com/documentation/full-text/phrase)          | `Phrase()`                                 |
| [Term](https://docs.paradedb.com/documentation/full-text/term)              | `Term()`                                   |
| [Highlighting](https://docs.paradedb.com/documentation/full-text/highlight) | `Snippet()`                                |
| [Proximity](https://docs.paradedb.com/documentation/full-text/proximity)    | `Proximity()`                              |
| [BM25 scoring](https://docs.paradedb.com/documentation/sorting/score)       | `Score()`                                  |
| [Tokenizers](https://docs.paradedb.com/documentation/tokenizers/overview)   | `TokenizeAsArray()`                        |

## Examples

- [Quickstart](examples/ParadeDB.EFCore.Quickstart/Program.cs)
- [Faceted Search](examples/ParadeDB.EFCore.FacetedSearch/Program.cs)
- [Autocomplete](examples/ParadeDB.EFCore.Autocomplete/Program.cs)
- [More Like This](examples/ParadeDB.EFCore.MoreLikeThis/Program.cs)
- [Hybrid Search (RRF)](examples/ParadeDB.EFCore.HybridRrf/Program.cs)
- [RAG](examples/ParadeDB.EFCore.Rag/Program.cs)

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for development setup, running tests, linting, and the PR workflow.

## Support

If you're missing a feature or have found a bug, please open a
[GitHub Issue](https://github.com/paradedb/efcore-paradedb/issues/new/choose).

To get community support, you can:

- Post a question in the [ParadeDB Slack Community](https://paradedb.com/slack)
- Ask for help on our [GitHub Discussions](https://github.com/paradedb/paradedb/discussions)

If you need commercial support, please [contact the ParadeDB team](mailto:sales@paradedb.com).

## Acknowledgments

We would like to thank the following members of the Entity Framework Core community for the initial implementation of this project:

- [Nandor Krizbai](https://github.com/nandor23) - .Net developer

## License

ParadeDB for Entity Framework Core is licensed under the [MIT License](LICENSE).

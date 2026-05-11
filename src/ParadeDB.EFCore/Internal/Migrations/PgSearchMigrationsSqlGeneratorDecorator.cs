using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace ParadeDB.EFCore.Internal.Migrations;

internal sealed class PgSearchMigrationsSqlGeneratorDecorator : IMigrationsSqlGenerator
{
    private const string ColumnTokenizersAnnotation = "ParadeDb:ColumnTokenizers";
    private const string SearchTokenizerAnnotation = "ParadeDb:SearchTokenizer";
    private const string AdditionalIndexColumnsAnnotation = "ParadeDb:AdditionalIndexColumns";
    private const string AdditionalColumnTokenizersAnnotation = "ParadeDb:AdditionalColumnTokenizers";
    private const string AdditionalIndexExpressionsAnnotation = "ParadeDb:AdditionalIndexExpressions";

    private readonly IMigrationsSqlGenerator _inner;
    private readonly MigrationsSqlGeneratorDependencies _dependencies;

    public PgSearchMigrationsSqlGeneratorDecorator(
        IMigrationsSqlGenerator inner,
        MigrationsSqlGeneratorDependencies dependencies)
    {
        _inner = inner;
        _dependencies = dependencies;
    }

    public IReadOnlyList<MigrationCommand> Generate(
        IReadOnlyList<MigrationOperation> operations,
        IModel? model = null,
        MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
    {
        var bm25Operations = new List<MigrationOperation>();
        var otherOperations = new List<MigrationOperation>();

        foreach (var operation in operations)
        {
            if (operation is CreateIndexOperation createIndex &&
                createIndex["Npgsql:IndexMethod"] is string method &&
                method == "bm25")
            {
                bm25Operations.Add(operation);
            }
            else
            {
                otherOperations.Add(operation);
            }
        }

        var commands = new List<MigrationCommand>(_inner.Generate(otherOperations, model, options));

        if (bm25Operations.Any())
        {
            commands.AddRange(GenerateBm25CreateIndex(bm25Operations));
        }

        return commands;
    }

    private IReadOnlyList<MigrationCommand> GenerateBm25CreateIndex(
        IReadOnlyList<MigrationOperation> operations)
    {
        var builder = new MigrationCommandListBuilder(_dependencies);
        var sqlHelper = _dependencies.SqlGenerationHelper;

        foreach (var operation in operations)
        {
            var createIndex = (CreateIndexOperation)operation;
            var tokenizers = createIndex[ColumnTokenizersAnnotation] as string[];
            var concurrently = createIndex["Npgsql:CreatedConcurrently"] as bool? == true;

            builder.Append("CREATE ");

            if (createIndex.IsUnique)
            {
                builder.Append("UNIQUE ");
            }

            builder.Append("INDEX ");

            if (concurrently)
            {
                builder.Append("CONCURRENTLY ");
            }

            builder.Append(sqlHelper.DelimitIdentifier(createIndex.Name))
                .Append(" ON ")
                .Append(sqlHelper.DelimitIdentifier(createIndex.Table, createIndex.Schema))
                .Append(" USING bm25 (");

            for (var i = 0; i < createIndex.Columns.Length; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                var columnName = sqlHelper.DelimitIdentifier(createIndex.Columns[i]);
                var tokenizer = tokenizers is not null && i < tokenizers.Length ? tokenizers[i] : null;

                if (string.IsNullOrEmpty(tokenizer))
                {
                    builder.Append(columnName);
                }
                else
                {
                    builder.Append("(").Append(columnName).Append("::").Append(tokenizer).Append(")");
                }
            }

            var additionalColumns = createIndex[AdditionalIndexColumnsAnnotation] as string[];
            var additionalTokenizers = createIndex[AdditionalColumnTokenizersAnnotation] as string[];

            if (additionalColumns is not null)
            {
                for (var i = 0; i < additionalColumns.Length; i++)
                {
                    builder.Append(", ");

                    var columnName = sqlHelper.DelimitIdentifier(additionalColumns[i]);
                    var tokenizer = additionalTokenizers is not null && i < additionalTokenizers.Length ? additionalTokenizers[i] : null;

                    if (string.IsNullOrEmpty(tokenizer))
                    {
                        builder.Append(columnName);
                    }
                    else
                    {
                        builder.Append("(").Append(columnName).Append("::").Append(tokenizer).Append(")");
                    }
                }
            }

            var additionalExpressions = createIndex[AdditionalIndexExpressionsAnnotation] as string[];

            if (additionalExpressions is not null)
            {
                for (var i = 0; i < additionalExpressions.Length; i++)
                {
                    builder.Append(", ");
                    builder.Append(additionalExpressions[i]);
                }
            }

            builder.Append(")");

            AppendStoreParameters(createIndex, builder);

            builder.AppendLine(";");
            builder.EndCommand(suppressTransaction: concurrently);
        }

        return builder.GetCommandList();
    }

    private static void AppendStoreParameters(CreateIndexOperation operation, MigrationCommandListBuilder builder)
    {
        var storageParameters = new Dictionary<string, object?>();

        foreach (var annotation in operation.GetAnnotations())
        {
            if (annotation.Name.StartsWith("Npgsql:StorageParameter:", StringComparison.Ordinal))
            {
                storageParameters[annotation.Name["Npgsql:StorageParameter:".Length..]] = annotation.Value;
            }
        }

        if (operation[SearchTokenizerAnnotation] is string searchTokenizer)
        {
            storageParameters["search_tokenizer"] = searchTokenizer;
        }

        if (storageParameters.Count > 0)
        {
            builder.Append(" WITH (");
            var first = true;
            foreach (var (key, value) in storageParameters)
            {
                if (!first)
                {
                    builder.Append(", ");
                }

                builder.Append(key).Append("=");

                if (value is string s)
                {
                    builder.Append("'").Append(s.Replace("'", "''")).Append("'");
                }
                else
                {
                    builder.Append(value?.ToString() ?? "");
                }

                first = false;
            }

            builder.Append(")");
        }
    }
}

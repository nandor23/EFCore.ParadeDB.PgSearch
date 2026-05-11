using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using ParadeDB.EFCore.BM25Attributes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using Npgsql.NameTranslation;
using ParadeDB.EFCore.Extensions;
using ParadeDB.EFCore.Internal.Storage;

namespace ParadeDB.EFCore.Internal.Query.Translators;

internal sealed class AliasTranslator : IMethodCallTranslator
{
    private static readonly NpgsqlSnakeCaseNameTranslator SnakeCase = new(null);

    private readonly ISqlExpressionFactory _sqlExpressionFactory;
    private readonly IModel _model;

    public AliasTranslator(
        ISqlExpressionFactory sqlExpressionFactory,
        ICurrentDbContext currentDbContext)
    {
        _sqlExpressionFactory = sqlExpressionFactory;
        _model = currentDbContext.Context.Model;
    }

    public SqlExpression? Translate(
        SqlExpression? instance,
        MethodInfo method,
        IReadOnlyList<SqlExpression> arguments,
        IDiagnosticsLogger<DbLoggerCategory.Query> logger
    )
    {
        if (method.Name != nameof(ParadeDbFunctionsExtensions.Alias))
        {
            return null;
        }

        string alias;

        if (arguments[2] is SqlConstantExpression { Value: string aliasValue })
        {
            alias = aliasValue;
        }
        else if (arguments.Count == 4
            && arguments[2] is SqlConstantExpression { Value: Tokenizer tokenizer }
            && arguments[3] is SqlConstantExpression { Value: int tokenizerIndex })
        {
            alias = ResolveAliasFromTokenizer(arguments[1], tokenizer, tokenizerIndex);
        }
        else if (arguments[2] is SqlConstantExpression { Value: Tokenizer tokenizerValue })
        {
            alias = ResolveAliasFromTokenizer(arguments[1], tokenizerValue);
        }
        else if (arguments.Count == 4
            && arguments[2] is SqlConstantExpression { Value: TokenizerType tokenizerType }
            && arguments[3] is SqlConstantExpression { Value: TokenFilter[] tokenFilters })
        {
            alias = ResolveAliasFromTokenizerType(arguments[1], tokenizerType, tokenFilters);
        }
        else if (arguments.Count == 4
            && arguments[2] is SqlConstantExpression { Value: TokenizerType tokenizerTypeWithIndex }
            && arguments[3] is SqlConstantExpression { Value: int tokenizerIndexValue })
        {
            alias = ResolveAliasFromTokenizerType(arguments[1], tokenizerTypeWithIndex, null, tokenizerIndexValue);
        }
        else if (arguments[2] is SqlConstantExpression { Value: TokenizerType tokenizerTypeOnly })
        {
            alias = ResolveAliasFromTokenizerType(arguments[1], tokenizerTypeOnly);
        }
        else
        {
            return null;
        }

        var typeMapping = new AliasTypeMapping(alias);

        return _sqlExpressionFactory.MakeUnary(
            ExpressionType.Convert,
            arguments[1],
            typeMapping.ClrType,
            typeMapping
        );
    }

    private string ResolveAliasFromTokenizer(
        SqlExpression propertyExpression,
        Tokenizer tokenizer,
        int? tokenizerIndex = null)
    {
        if (propertyExpression is JsonScalarExpression jsonScalar)
        {
            return ResolveJsonAlias(jsonScalar, tokenizer, tokenizerIndex);
        }

        if (propertyExpression is ColumnExpression column)
        {
            return ResolveColumnAlias(column, tokenizer, tokenizerIndex);
        }

        throw new InvalidOperationException("Cannot resolve column name from SQL expression.");
    }

    private string ResolveAliasFromTokenizerType(
        SqlExpression propertyExpression,
        TokenizerType tokenizerType,
        TokenFilter[]? tokenFilters = null,
        int? tokenizerIndex = null)
    {
        var attributeType = TokenizerTypeMappings.GetAttributeType(tokenizerType);
        var tokenizerName = attributeType.Name
            .Replace("BM25Tokenizer", "", StringComparison.Ordinal)
            .Replace("Attribute", "", StringComparison.Ordinal)
            .ToLowerInvariant();

        if (propertyExpression is JsonScalarExpression jsonScalar)
        {
            return ResolveJsonAlias(jsonScalar, attributeType, tokenizerName, tokenFilters, tokenizerIndex);
        }

        if (propertyExpression is ColumnExpression column)
        {
            return ResolveColumnAlias(column, attributeType, tokenizerName, tokenFilters, tokenizerIndex);
        }

        throw new InvalidOperationException("Cannot resolve column name from SQL expression.");
    }

    private string ResolveColumnAlias(ColumnExpression column, Tokenizer tokenizer, int? tokenizerIndex)
    {
        var name = column.Name;

        if (TryFindPropertyByColumnName(name, out var property) && property is not null)
        {
            return ResolveAliasFromTokenizerAttributes(property, name, tokenizer, tokenizerIndex);
        }

        throw new InvalidOperationException($"Cannot resolve property metadata for column '{name}'.");
    }

    private string ResolveColumnAlias(
        ColumnExpression column,
        Type attributeType,
        string tokenizerName,
        TokenFilter[]? tokenFilters,
        int? tokenizerIndex)
    {
        var name = column.Name;

        if (TryFindPropertyByColumnName(name, out var property) && property is not null)
        {
            var resolved = ResolveAliasFromAttributes(
                property,
                attributeType,
                name,
                tokenFilters,
                tokenizerIndex);

            return resolved ?? name;
        }

        return $"{name}_{tokenizerName}";
    }

    private string ResolveJsonAlias(
        JsonScalarExpression jsonScalar,
        Tokenizer tokenizer,
        int? tokenizerIndex)
    {
        var jsonColumn = jsonScalar.Json as ColumnExpression;
        var columnName = jsonColumn?.Name;
        var propertyName = jsonScalar.Path.LastOrDefault().PropertyName;

        if (columnName is not null
            && propertyName is not null
            && TryFindJsonProperty(columnName, propertyName, out var property)
            && property is not null)
        {
            var aliasBase = $"{columnName}_{SnakeCase.TranslateMemberName(propertyName)}";
            return ResolveAliasFromTokenizerAttributes(property, aliasBase, tokenizer, tokenizerIndex);
        }

        throw new InvalidOperationException("Cannot resolve property metadata from JSON SQL expression.");
    }

    private string ResolveJsonAlias(
        JsonScalarExpression jsonScalar,
        Type attributeType,
        string tokenizerName,
        TokenFilter[]? tokenFilters,
        int? tokenizerIndex)
    {
        var jsonColumn = jsonScalar.Json as ColumnExpression;
        var columnName = jsonColumn?.Name;
        var propertyName = jsonScalar.Path.LastOrDefault().PropertyName;

        if (columnName is not null
            && propertyName is not null
            && TryFindJsonProperty(columnName, propertyName, out var property)
            && property is not null)
        {
            var aliasBase = $"{columnName}_{SnakeCase.TranslateMemberName(propertyName)}";
            var resolved = ResolveAliasFromAttributes(
                property,
                attributeType,
                aliasBase,
                tokenFilters,
                tokenizerIndex);

            return resolved ?? aliasBase;
        }

        return tokenizerName;
    }

    private static string? ResolveAliasFromAttributes(
        IProperty property,
        Type attributeType,
        string aliasBase,
        TokenFilter[]? tokenFilters,
        int? tokenizerIndex)
    {
        var memberInfo = (MemberInfo?)property.PropertyInfo ?? property.FieldInfo;
        if (memberInfo is null)
        {
            return null;
        }

        var allTokenizers = memberInfo.GetCustomAttributes<BM25TokenizerAttribute>(inherit: true).ToArray();
        var matchingType = allTokenizers.Where(t => t.GetType() == attributeType).ToArray();

        if (matchingType.Length == 0)
        {
            return null;
        }

        BM25TokenizerAttribute matchingTokenizer;

        if (tokenizerIndex.HasValue)
        {
            if (tokenizerIndex < 0 || tokenizerIndex >= matchingType.Length)
            {
                throw new InvalidOperationException(
                    $"Tokenizer index {tokenizerIndex} is out of range for property '{property.Name}' and tokenizer '{attributeType.Name}'.");
            }

            matchingTokenizer = matchingType[tokenizerIndex.Value];
        }
        else if (tokenFilters is not null)
        {
            var matchingFilters = matchingType
                .Where(t => HaveSameFilters(t.GetTokenFilters(), tokenFilters))
                .ToArray();

            if (matchingFilters.Length > 1)
            {
                throw new InvalidOperationException(
                    $"Multiple tokenizers '{attributeType.Name}' on property '{property.Name}' match the requested token filters. Use the tokenizer index overload instead.");
            }

            if (matchingFilters.Length == 0)
            {
                throw new InvalidOperationException(
                    $"No tokenizer '{attributeType.Name}' on property '{property.Name}' matches the requested token filters.");
            }

            matchingTokenizer = matchingFilters[0];
        }
        else
        {
            matchingTokenizer = matchingType[0];
        }

        return BuildAlias(allTokenizers, matchingTokenizer, aliasBase);
    }

    private static string ResolveAliasFromTokenizerAttributes(
        IProperty property,
        string aliasBase,
        Tokenizer tokenizer,
        int? tokenizerIndex)
    {
        var memberInfo = (MemberInfo?)property.PropertyInfo ?? property.FieldInfo;
        if (memberInfo is null)
        {
            throw new InvalidOperationException(
                $"Cannot resolve member metadata for property '{property.Name}'.");
        }

        var allTokenizers = memberInfo.GetCustomAttributes<BM25TokenizerAttribute>(inherit: true).ToArray();
        var requestedTokenizer = tokenizer.ToString(searchTokenizer: true);
        var matching = allTokenizers
            .Where(t => t.ToTokenizer().ToString(searchTokenizer: true) == requestedTokenizer)
            .ToArray();

        if (matching.Length == 0)
        {
            throw new InvalidOperationException(
                $"No tokenizer on property '{property.Name}' matches the requested tokenizer '{requestedTokenizer}'.");
        }

        BM25TokenizerAttribute matchingTokenizer;

        if (tokenizerIndex.HasValue)
        {
            if (tokenizerIndex < 0 || tokenizerIndex >= matching.Length)
            {
                throw new InvalidOperationException(
                    $"Tokenizer index {tokenizerIndex} is out of range for property '{property.Name}' and tokenizer '{requestedTokenizer}'.");
            }

            matchingTokenizer = matching[tokenizerIndex.Value];
        }
        else
        {
            matchingTokenizer = matching[0];
        }

        return BuildAlias(allTokenizers, matchingTokenizer, aliasBase) ?? aliasBase;
    }

    private static string? BuildAlias(
        IReadOnlyList<BM25TokenizerAttribute> allTokenizers,
        BM25TokenizerAttribute matchingTokenizer,
        string aliasBase)
    {
        if (!string.IsNullOrEmpty(matchingTokenizer.AliasSuffix))
        {
            return $"{aliasBase}_{matchingTokenizer.AliasSuffix}";
        }

        return null;
    }

    private static bool HaveSameFilters(
        IReadOnlyList<TokenFilter> left,
        IReadOnlyList<TokenFilter> right)
    {
        if (left.Count != right.Count)
        {
            return false;
        }

        for (var i = 0; i < left.Count; i++)
        {
            if (!Equals(left[i], right[i]))
            {
                return false;
            }
        }

        return true;
    }

    private bool TryFindPropertyByColumnName(string columnName, out IProperty? property)
    {
        property = null;

        foreach (var entityType in _model.GetEntityTypes())
        {
            foreach (var modelProperty in entityType.GetProperties())
            {
                if (modelProperty.GetColumnName() == columnName)
                {
                    property = modelProperty;
                    return true;
                }
            }
        }

        return false;
    }

    private bool TryFindJsonProperty(
        string columnName,
        string jsonPropertyName,
        out IProperty? property)
    {
        property = null;

        foreach (var entityType in _model.GetEntityTypes())
        {
            if (TryFindJsonPropertyInEntity(entityType, columnName, jsonPropertyName, out property))
            {
                return true;
            }
        }

        return false;
    }

    private static bool TryFindJsonPropertyInEntity(
        IEntityType entityType,
        string columnName,
        string jsonPropertyName,
        out IProperty? property)
    {
        property = null;

        foreach (var navigation in entityType.GetNavigations())
        {
            if (navigation.ForeignKey.IsOwnership)
            {
                var target = navigation.TargetEntityType;

                if (target.IsMappedToJson()
                    && target.GetContainerColumnName() == columnName
                    && TryFindJsonPropertyInOwnedType(target, jsonPropertyName, out property))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool TryFindJsonPropertyInOwnedType(
        IEntityType ownedType,
        string jsonPropertyName,
        out IProperty? property)
    {
        property = null;

        foreach (var modelProperty in ownedType.GetProperties())
        {
            var propertyName = modelProperty.GetJsonPropertyName() ?? modelProperty.Name;

            if (propertyName == jsonPropertyName)
            {
                property = modelProperty;
                return true;
            }
        }

        foreach (var navigation in ownedType.GetNavigations())
        {
            if (navigation.ForeignKey.IsOwnership
                && TryFindJsonPropertyInOwnedType(navigation.TargetEntityType, jsonPropertyName, out property))
            {
                return true;
            }
        }

        return false;
    }
}

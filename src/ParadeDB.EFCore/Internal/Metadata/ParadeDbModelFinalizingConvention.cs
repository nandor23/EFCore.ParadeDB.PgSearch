using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

using Npgsql.NameTranslation;

using NpgsqlTypes;

using ParadeDB.EFCore.BM25Attributes;

namespace ParadeDB.EFCore.Internal.Metadata;

internal sealed class ParadeDbModelFinalizingConvention : IModelFinalizingConvention
{
    private static readonly HashSet<Type> ValidTokenizerTypes =
    [
        typeof(string),
        typeof(char),
        typeof(Rune)
    ];

    private static readonly HashSet<Type> ParadeDbSupportedScalarTypes =
    [
        typeof(string), typeof(char), typeof(Rune),
        typeof(bool),
        typeof(byte), typeof(sbyte), typeof(short), typeof(int), typeof(long),
        typeof(ushort), typeof(uint), typeof(ulong),
        typeof(float), typeof(double), typeof(decimal), typeof(BigInteger),
        typeof(Guid),
        typeof(IPAddress), typeof(NpgsqlInet),
        typeof(DateTime), typeof(DateTimeOffset), typeof(DateOnly), typeof(TimeOnly), typeof(TimeSpan)
    ];

    private static readonly NpgsqlSnakeCaseNameTranslator SnakeCaseNameTranslator = new();

    public void ProcessModelFinalizing(
        IConventionModelBuilder modelBuilder,
        IConventionContext<IConventionModelBuilder> context
    )
    {
        
        // Code initially based on
        // https://github.com/daniel3303/ParadeDbEntityFrameworkCore/blob/765fc44da85dca1400682d70c5ae2e4a10900501/Equibles.ParadeDB.EntityFrameworkCore/ParadeDbModelFinalizingConvention.cs
        
        bool hasBm25Index = false;

        foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            if (entityType.FindOwnership() is not null)
            {
                continue;
            }

            BM25IndexAttribute? attribute = entityType.ClrType.GetCustomAttribute<BM25IndexAttribute>();
            if (attribute is null)
            {
                continue;
            }

            IConventionKey? pk = entityType.FindPrimaryKey();

            if (pk is null)
            {
                throw new InvalidOperationException(
                    $"Entity type '{entityType.ClrType.Name}' must have a primary key defined to use BM25Index attribute."
                );
            }

            if (pk.Properties.Count != 1)
            {
                throw new InvalidOperationException(
                    $"Entity type '{entityType.ClrType.Name}' must have a single-column primary key to use BM25Index attribute."
                );
            }

            IConventionProperty keyProperty = pk.Properties.First();
            string keyColumnName = keyProperty.GetColumnName();

            List<string> indexPropertyNames = [keyProperty.Name!];
            List<string?> tokenizers = [null];
            List<string> additionalColumnNames = [];
            List<string?> additionalColumnTokenizers = [];
            List<string> additionalIndexExpressions = [];

            BM25TokenizerAttribute[] inheritedTokenizers =
                entityType.ClrType.GetCustomAttributes<BM25TokenizerAttribute>(true).ToArray();
            bool classIndexAll = ClassHasIndexAllAttribute(entityType.ClrType);

            ProcessDirectProperties(entityType, inheritedTokenizers, classIndexAll, indexPropertyNames, tokenizers,
                additionalColumnNames, additionalColumnTokenizers);

            foreach (IConventionNavigation navigation in entityType.GetNavigations())
            {
                if (!navigation.ForeignKey.IsOwnership)
                {
                    continue;
                }

                ProcessOwnedNavigation(
                    navigation,
                    additionalColumnNames,
                    additionalColumnTokenizers,
                    additionalIndexExpressions);
            }

            if (!indexPropertyNames.Skip(1).Any()
                && !additionalColumnNames.Any()
                && !additionalIndexExpressions.Any())
            {
                continue;
            }

            IConventionIndexBuilder? indexBuilder = entityType.Builder.HasIndex(
                indexPropertyNames,
                true
            );

            if (indexBuilder is null)
            {
                throw new InvalidOperationException(
                    $"HasIndex returned null for entity '{entityType.ClrType.Name}' " +
                    $"with properties: {string.Join(", ", indexPropertyNames)}"
                );
            }

            indexBuilder.HasAnnotation("Npgsql:IndexMethod", "bm25", true);
            indexBuilder.HasAnnotation("Npgsql:StorageParameter:key_field", keyColumnName, true);

            if (!string.IsNullOrEmpty(attribute.SearchTokenizer))
            {
                indexBuilder.HasAnnotation("ParadeDb:SearchTokenizer", attribute.SearchTokenizer, true);
            }

            if (tokenizers.Any(t => t is not null))
            {
                indexBuilder.HasAnnotation("ParadeDb:ColumnTokenizers", tokenizers.ToArray(), true);
            }

            if (additionalColumnNames.Count > 0)
            {
                indexBuilder.HasAnnotation("ParadeDb:AdditionalIndexColumns", additionalColumnNames.ToArray(), true);

                if (additionalColumnTokenizers.Any(t => t is not null))
                {
                    indexBuilder.HasAnnotation("ParadeDb:AdditionalColumnTokenizers",
                        additionalColumnTokenizers.ToArray(), true);
                }
            }

            if (additionalIndexExpressions.Count > 0)
            {
                indexBuilder.HasAnnotation("ParadeDb:AdditionalIndexExpressions", additionalIndexExpressions.ToArray(),
                    true);
            }

            hasBm25Index = true;
        }

        if (hasBm25Index)
        {
            modelBuilder.HasPostgresExtension("pg_search");
        }
    }

    private static void ProcessDirectProperties(
        IConventionEntityType entityType,
        BM25TokenizerAttribute[] classTokenizers,
        bool classIndexAll,
        List<string> indexPropertyNames,
        List<string?> tokenizers,
        List<string> additionalColumnNames,
        List<string?> additionalColumnTokenizers)
    {
        IConventionKey? pk = entityType.FindPrimaryKey();

        foreach (IConventionProperty prop in entityType.GetProperties())
        {
            if (prop == pk?.Properties[0])
            {
                continue;
            }

            var propTokenizers = GetTokenizerAttributes(prop);
            bool isIndexable = IsIndexable(prop);

            if (propTokenizers.Count > 0)
            {
                ApplyTokenizers(
                    prop,
                    propTokenizers,
                    indexPropertyNames,
                    tokenizers,
                    additionalColumnNames,
                    additionalColumnTokenizers);
            }
            else if (isIndexable)
            {
                indexPropertyNames.Add(prop.Name!);
                tokenizers.Add(null);
            }
            else if (classIndexAll && IsParadeDbSupportedType(prop.ClrType))
            {
                if (classTokenizers.Length > 0)
                {
                    ApplyTokenizers(
                        prop,
                        classTokenizers,
                        indexPropertyNames,
                        tokenizers,
                        additionalColumnNames,
                        additionalColumnTokenizers);
                }
                else
                {
                    indexPropertyNames.Add(prop.Name!);
                    tokenizers.Add(null);
                }
            }
        }
    }

    private static void ProcessOwnedNavigation(
        IConventionNavigation rootNavigation,
        List<string> additionalColumnNames,
        List<string?> additionalColumnTokenizers,
        List<string> additionalIndexExpressions)
    {
        IConventionEntityType targetEntityType = rootNavigation.TargetEntityType;
        BM25TokenizerAttribute[] classTokenizers =
            targetEntityType.ClrType.GetCustomAttributes<BM25TokenizerAttribute>(true).ToArray();
        bool classIndexAll = ClassHasIndexAllAttribute(targetEntityType.ClrType);
        bool isJson = targetEntityType.IsMappedToJson();

        HashSet<IConventionProperty> overriddenProperties = [];

        foreach (IConventionProperty prop in targetEntityType.GetProperties())
        {
            var propTokenizers = GetTokenizerAttributes(prop);
            bool isIndexable = IsIndexable(prop);

            if (propTokenizers.Count > 0)
            {
                overriddenProperties.Add(prop);
                ApplyTokenizers(
                    rootNavigation,
                    prop,
                    propTokenizers,
                    isJson,
                    additionalColumnNames,
                    additionalColumnTokenizers,
                    additionalIndexExpressions);
            }
            else if (isIndexable)
            {
                overriddenProperties.Add(prop);
                AddProperty(rootNavigation, prop, null, isJson,
                    additionalColumnNames, additionalColumnTokenizers, additionalIndexExpressions);
            }
        }

        foreach (IConventionProperty prop in targetEntityType.GetProperties())
        {
            if (overriddenProperties.Contains(prop) || prop.IsKey())
            {
                continue;
            }

            if (!classIndexAll && !IsIndexable(prop))
            {
                continue;
            }

            if (!IsParadeDbSupportedType(prop.ClrType))
            {
                continue;
            }

            if (classTokenizers.Length > 0)
            {
                ApplyTokenizers(
                    rootNavigation,
                    prop,
                    classTokenizers,
                    isJson,
                    additionalColumnNames,
                    additionalColumnTokenizers,
                    additionalIndexExpressions);
            }
            else
            {
                AddProperty(rootNavigation, prop, null, isJson,
                    additionalColumnNames, additionalColumnTokenizers, additionalIndexExpressions);
            }
        }

        foreach (IConventionNavigation navigation in targetEntityType.GetNavigations())
        {
            if (!navigation.ForeignKey.IsOwnership)
            {
                continue;
            }

            ProcessOwnedNavigation(
                navigation,
                additionalColumnNames,
                additionalColumnTokenizers,
                additionalIndexExpressions);
        }
    }

    private static void AddProperty(
        IConventionNavigation rootNavigation,
        IConventionProperty prop,
        string? tokenizer,
        bool isJson,
        List<string> additionalColumnNames,
        List<string?> additionalColumnTokenizers,
        List<string> additionalIndexExpressions)
    {
        if (isJson)
        {
            string jsonPath = BuildJsonPath(rootNavigation, prop);
            additionalIndexExpressions.Add(string.IsNullOrEmpty(tokenizer)
                ? jsonPath
                : $"(({jsonPath})::{tokenizer})");
        }
        else
        {
            additionalColumnNames.Add(prop.GetColumnName()!);
            additionalColumnTokenizers.Add(tokenizer);
        }
    }

    private static void ApplyTokenizers(
        IConventionProperty prop,
        IReadOnlyList<BM25TokenizerAttribute> tokenizers,
        List<string> indexPropertyNames,
        List<string?> indexTokenizers,
        List<string> additionalColumnNames,
        List<string?> additionalColumnTokenizers)
    {
        if (tokenizers.Count == 0)
        {
            return;
        }

        if (!IsValidTokenizerType(prop.ClrType))
        {
            indexPropertyNames.Add(prop.Name!);
            indexTokenizers.Add(null);
            return;
        }

        var columnName = prop.GetColumnName()!;
        var aliasBase = SnakeCaseNameTranslator.TranslateMemberName(columnName);

        ValidateAndThrowForDuplicates(tokenizers, prop.Name!);

        for (var i = 0; i < tokenizers.Count; i++)
        {
            var tokenizer = tokenizers[i].ToTokenizer();
            string? alias = ResolveAlias(tokenizers[i], aliasBase);

            if (alias is not null)
            {
                tokenizer = tokenizer.WithAlias(alias);
            }

            var tokenizerString = tokenizer.ToString();
            if (i == 0)
            {
                indexPropertyNames.Add(prop.Name!);
                indexTokenizers.Add(tokenizerString);
            }
            else
            {
                additionalColumnNames.Add(columnName);
                additionalColumnTokenizers.Add(tokenizerString);
            }
        }
    }

    private static void ApplyTokenizers(
        IConventionNavigation rootNavigation,
        IConventionProperty prop,
        IReadOnlyList<BM25TokenizerAttribute> tokenizers,
        bool isJson,
        List<string> additionalColumnNames,
        List<string?> additionalColumnTokenizers,
        List<string> additionalIndexExpressions)
    {
        if (tokenizers.Count == 0)
        {
            return;
        }

        if (!IsValidTokenizerType(prop.ClrType))
        {
            AddProperty(rootNavigation, prop, null, isJson,
                additionalColumnNames, additionalColumnTokenizers, additionalIndexExpressions);
            return;
        }

        string baseAlias = isJson
            ? BuildAliasBase(rootNavigation, prop)
            : prop.GetColumnName()!;

        ValidateAndThrowForDuplicates(tokenizers, prop.Name!);

        for (var i = 0; i < tokenizers.Count; i++)
        {
            var tokenizer = tokenizers[i].ToTokenizer();
            string? alias = ResolveAlias(tokenizers[i], baseAlias);

            if (alias is not null)
            {
                tokenizer = tokenizer.WithAlias(alias);
            }
            else if (isJson)
            {
                tokenizer = tokenizer.WithAlias(baseAlias);
            }

            AddProperty(rootNavigation, prop, tokenizer.ToString(), isJson,
                additionalColumnNames, additionalColumnTokenizers, additionalIndexExpressions);
        }
    }

    private static void ValidateAndThrowForDuplicates(IReadOnlyList<BM25TokenizerAttribute> tokenizers, string propertyName)
    {
        if (tokenizers.Count <= 1)
        {
            return;
        }

        var tokenizersWithoutAliasSuffix = tokenizers.Count(t => string.IsNullOrEmpty(t.AliasSuffix));
        if (tokenizersWithoutAliasSuffix > 1)
        {
            throw new InvalidOperationException(
                $"Only one tokenizer without an explicit alias suffix is allowed on property '{propertyName}'. " +
                "Set the 'AliasSuffix' property on the remaining tokenizer attributes.");
        }
    }

    private static string? ResolveAlias(BM25TokenizerAttribute tokenizerAttr, string aliasBase)
    {
        if (!string.IsNullOrEmpty(tokenizerAttr.AliasSuffix))
        {
            return $"{aliasBase}_{tokenizerAttr.AliasSuffix}";
        }

        return null;
    }

    private static string BuildJsonPath(IConventionNavigation rootNavigation, IConventionProperty leafProperty)
    {
        string column = rootNavigation.TargetEntityType.GetContainerColumnName()!;

        List<string> segments = [];

        if (rootNavigation.DeclaringEntityType.IsMappedToJson())
        {
            string rootName = rootNavigation.TargetEntityType.GetJsonPropertyName() ?? rootNavigation.Name!;
            segments.Add(rootName);
        }

        List<string> intermediateSegments = [];
        IConventionEntityType? currentEntityType = leafProperty.DeclaringType as IConventionEntityType;

        while (currentEntityType != null && currentEntityType != rootNavigation.TargetEntityType)
        {
            IConventionForeignKey? ownership = currentEntityType.FindOwnership();
            if (ownership is null)
            {
                break;
            }

            string? jsonName = currentEntityType.GetJsonPropertyName();
            if (string.IsNullOrEmpty(jsonName))
            {
                break;
            }

            intermediateSegments.Add(jsonName);
            currentEntityType = ownership.PrincipalEntityType;
        }

        intermediateSegments.Reverse();
        segments.AddRange(intermediateSegments);

        string jsonPropName = leafProperty.GetJsonPropertyName() ?? leafProperty.Name!;

        string path = column;
        foreach (string segment in segments)
        {
            path += $"->'{segment}'";
        }

        path += $"->>'{jsonPropName}'";

        return path;
    }

    private static bool ClassHasIndexAllAttribute(Type type)
    {
        return type.GetCustomAttribute<BM25DefaultTokenizerAttribute>(true) is not null
               || type.GetCustomAttribute<BM25TokenizerAttribute>(true) is not null;
    }

    private enum TypeSupport
    {
        Invalid,
        Valid,
        ValidForTokenizer
    }

    private static bool IsParadeDbSupportedType(Type type) => GetTypeSupport(type) != TypeSupport.Invalid;

    private static bool IsValidTokenizerType(Type type) => GetTypeSupport(type) == TypeSupport.ValidForTokenizer;

    private static TypeSupport GetTypeSupport(Type type)
    {
        if (ValidTokenizerTypes.Contains(type))
            return TypeSupport.ValidForTokenizer;

        if (ParadeDbSupportedScalarTypes.Contains(type))
            return TypeSupport.Valid;

        var elementType = GetEnumerableElementType(type);
        if (elementType is null)
            return TypeSupport.Invalid;

        if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
        {
            var args = elementType.GetGenericArguments();
            if (args[0] != typeof(string))
                return TypeSupport.Invalid;

            var valueSupport = GetTypeSupport(args[1]);
            return valueSupport != TypeSupport.Invalid ? TypeSupport.ValidForTokenizer : TypeSupport.Invalid;
        }

        return GetTypeSupport(elementType);
    }

    private static Type? GetEnumerableElementType(Type type)
    {
        if (type.IsArray)
            return type.GetElementType();

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            return type.GetGenericArguments()[0];

        foreach (var iface in type.GetInterfaces())
        {
            if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return iface.GetGenericArguments()[0];
        }

        return null;
    }

    private static IReadOnlyList<BM25TokenizerAttribute> GetTokenizerAttributes(IConventionProperty property)
    {
        MemberInfo? member = (MemberInfo?)property.PropertyInfo ?? property.FieldInfo;
        return member?.GetCustomAttributes<BM25TokenizerAttribute>(true).ToArray()
               ?? Array.Empty<BM25TokenizerAttribute>();
    }

    private static string BuildAliasBase(IConventionNavigation rootNavigation, IConventionProperty leafProperty)
    {
        var column = rootNavigation.TargetEntityType.GetContainerColumnName()!;
        var leafName = leafProperty.GetJsonPropertyName() ?? leafProperty.Name!;
        var translatedLeafName = SnakeCaseNameTranslator.TranslateMemberName(leafName);
        return $"{column}_{translatedLeafName}";
    }

    private static bool IsIndexable(IConventionProperty property)
    {
        MemberInfo? member = (MemberInfo?)property.PropertyInfo ?? property.FieldInfo;
        return member?.GetCustomAttribute<BM25DefaultTokenizerAttribute>(true) is not null;
    }
}

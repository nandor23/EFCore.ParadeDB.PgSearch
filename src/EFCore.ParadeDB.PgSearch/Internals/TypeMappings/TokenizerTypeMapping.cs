using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internals.TypeMappings;

internal sealed class TokenizerTypeMapping : RelationalTypeMapping
{
    public TokenizerTypeMapping(Tokenizer tokenizer)
        : base($"{tokenizer}::text[]", typeof(string[])) { }

    private TokenizerTypeMapping(RelationalTypeMappingParameters parameters)
        : base(parameters) { }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
    {
        return new TokenizerTypeMapping(parameters);
    }
}

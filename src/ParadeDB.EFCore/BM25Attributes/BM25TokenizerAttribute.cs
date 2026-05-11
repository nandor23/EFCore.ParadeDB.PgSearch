using ParadeDB.EFCore.BM25Attributes.Enums;

namespace ParadeDB.EFCore.BM25Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public sealed class BM25DefaultTokenizerAttribute : Attribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public abstract class BM25TokenizerAttribute : Attribute
{
    public string? AliasSuffix { get; init; }

    public bool AlphaNumericOnly { get; init; }
    public bool AsciiFolding { get; init; }
    public bool PreserveCase { get; init; }
    public bool Trim { get; init; }

    public StemmerLanguage Stemmer { get; init; } = (StemmerLanguage)(-1);
    public int RemoveLong { get; init; } = -1;
    public int RemoveShort { get; init; } = -1;
    public StopwordsLanguage[] RemoveStopwords { get; init; } = [];

    public abstract Tokenizer ToTokenizer();

    internal TokenFilter[] GetTokenFilters()
    {
        List<TokenFilter> filters = [];

        if (AlphaNumericOnly)
        {
            filters.Add(TokenFilter.AlphaNumericOnly);
        }

        if (AsciiFolding)
        {
            filters.Add(TokenFilter.AsciiFolding);
        }

        if (PreserveCase)
        {
            filters.Add(TokenFilter.PreserveCase);
        }

        if (Trim)
        {
            filters.Add(TokenFilter.Trim);
        }

        if ((int)Stemmer >= 0)
        {
            filters.Add(TokenFilter.Stemmer(Stemmer));
        }

        if (RemoveLong >= 0)
        {
            filters.Add(TokenFilter.RemoveLong(RemoveLong));
        }

        if (RemoveShort >= 0)
        {
            filters.Add(TokenFilter.RemoveShort(RemoveShort));
        }

        if (RemoveStopwords.Length > 0)
        {
            filters.Add(TokenFilter.RemoveStopwords(RemoveStopwords));
        }

        return filters.ToArray();
    }
}

public sealed class BM25WhitespaceTokenizerAttribute : BM25TokenizerAttribute
{
    public override Tokenizer ToTokenizer() => Tokenizer.Whitespace(GetTokenFilters());
}

public sealed class BM25UnicodeTokenizerAttribute : BM25TokenizerAttribute
{
    public bool RemoveEmoji { get; set; } = false;

    public override Tokenizer ToTokenizer() =>
        RemoveEmoji
            ? Tokenizer.Unicode(true, GetTokenFilters())
            : Tokenizer.Unicode(GetTokenFilters());
}

public sealed class BM25SimpleTokenizerAttribute : BM25TokenizerAttribute
{
    public override Tokenizer ToTokenizer() => Tokenizer.Simple(GetTokenFilters());
}

public sealed class BM25RegexPatternTokenizerAttribute : BM25TokenizerAttribute
{
    public string Pattern { get; }

    public BM25RegexPatternTokenizerAttribute(string pattern)
    {
        Pattern = pattern;
    }

    public override Tokenizer ToTokenizer() => Tokenizer.RegexPattern(Pattern, GetTokenFilters());
}

public sealed class BM25SourceCodeTokenizerAttribute : BM25TokenizerAttribute
{
    public override Tokenizer ToTokenizer() => Tokenizer.SourceCode(GetTokenFilters());
}

public sealed class BM25LiteralTokenizerAttribute : BM25TokenizerAttribute
{
    public override Tokenizer ToTokenizer() => Tokenizer.Literal;
}

public sealed class BM25LiteralNormalizedTokenizerAttribute : BM25TokenizerAttribute
{
    public override Tokenizer ToTokenizer() => Tokenizer.LiteralNormalized(GetTokenFilters());
}

public sealed class BM25EdgeNgramTokenizerAttribute : BM25TokenizerAttribute
{
    public int MinGram { get; }
    public int MaxGram { get; }
    public TokenChars[] TokenCharsFlags { get; }

    public BM25EdgeNgramTokenizerAttribute(int minGram, int maxGram, params TokenChars[] tokenCharsFlags)
    {
        MinGram = minGram;
        MaxGram = maxGram;
        TokenCharsFlags = tokenCharsFlags;
    }

    public override Tokenizer ToTokenizer()
    {
        if (TokenCharsFlags.Length == 0)
            return Tokenizer.EdgeNgram(MinGram, MaxGram, GetTokenFilters());

        var combined = TokenCharsFlags.Aggregate((a, b) => a | b);
        return Tokenizer.EdgeNgram(MinGram, MaxGram, combined, GetTokenFilters());
    }
}

public sealed class BM25NgramTokenizerAttribute : BM25TokenizerAttribute
{
    public int MinGram { get; }
    public int MaxGram { get; }
    public NgramMode[] Modes { get; }

    public BM25NgramTokenizerAttribute(int minGram, int maxGram, params NgramMode[] modes)
    {
        MinGram = minGram;
        MaxGram = maxGram;
        Modes = modes;
    }

    public override Tokenizer ToTokenizer() =>
        Modes.Length switch
        {
            0 => Tokenizer.Ngram(MinGram, MaxGram, GetTokenFilters()),
            1 when Modes[0] == NgramMode.Positions => Tokenizer.NgramPositions(MinGram, GetTokenFilters()),
            1 when Modes[0] == NgramMode.PrefixOnly => Tokenizer.NgramPrefixOnly(MinGram, MaxGram, GetTokenFilters()),
            _ => throw new InvalidOperationException(
                $"Invalid NgramMode combination: {string.Join(", ", Modes)}")
        };
}

public sealed class BM25IcuTokenizerAttribute : BM25TokenizerAttribute
{
    public override Tokenizer ToTokenizer() => Tokenizer.Icu(GetTokenFilters());
}

public sealed class BM25JiebaTokenizerAttribute : BM25TokenizerAttribute
{
    public override Tokenizer ToTokenizer() => Tokenizer.Jieba(GetTokenFilters());
}

public sealed class BM25LinderaTokenizerAttribute : BM25TokenizerAttribute
{
    public LinderaLanguage Language { get; }
    public bool KeepWhitespace { get; }

    public BM25LinderaTokenizerAttribute(LinderaLanguage language, bool keepWhitespace = false)
    {
        Language = language;
        KeepWhitespace = keepWhitespace;
    }

    public override Tokenizer ToTokenizer() =>
        KeepWhitespace
            ? Tokenizer.Lindera(Language, true, GetTokenFilters())
            : Tokenizer.Lindera(Language, GetTokenFilters());
}

public sealed class BM25ChineseCompatibleTokenizerAttribute : BM25TokenizerAttribute
{
    public override Tokenizer ToTokenizer() => Tokenizer.ChineseCompatible(GetTokenFilters());
}

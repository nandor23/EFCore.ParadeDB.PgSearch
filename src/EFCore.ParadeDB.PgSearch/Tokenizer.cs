using System.Diagnostics.CodeAnalysis;
using EFCore.ParadeDB.PgSearch.Internals.Tokenizers;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.ParadeDB.PgSearch;

public abstract class Tokenizer
{
    internal abstract SqlExpression ToSqlExpression();

    public static Tokenizer Default { get; } = new BasicTokenizer("default");
    public static Tokenizer Whitespace { get; } = new BasicTokenizer("whitespace");
    public static Tokenizer Raw { get; } = new BasicTokenizer("raw");
    public static Tokenizer Keyword { get; } = new BasicTokenizer("keyword");
    public static Tokenizer SourceCode { get; } = new BasicTokenizer("source_code");
    public static Tokenizer ChineseCompatible { get; } = new BasicTokenizer("chinese_compatible");
    public static Tokenizer ChineseLindera { get; } = new BasicTokenizer("chinese_lindera");
    public static Tokenizer KoreanLindera { get; } = new BasicTokenizer("korean_lindera");
    public static Tokenizer JapaneseLindera { get; } = new BasicTokenizer("japanese_lindera");
    public static Tokenizer Jieba { get; } = new BasicTokenizer("jieba");
    public static Tokenizer Icu { get; } = new BasicTokenizer("icu");

    public static Tokenizer Regex([StringSyntax(StringSyntaxAttribute.Regex)] string pattern) =>
        new RegexTokenizer(pattern);

    public static Tokenizer Ngram(int minGram, int maxGram, bool prefixOnly) =>
        new NgramTokenizer(minGram, maxGram, prefixOnly);
}

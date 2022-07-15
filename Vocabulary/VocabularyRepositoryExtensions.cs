using System.Collections.Generic;
using System.Linq;

namespace Vocabulary;

public static class VocabularyRepositoryExtensions
{
    public static IEnumerable<string> FindWordsByMeaning(this IReadOnlyList<WordWithMeanings> vocabulary, string meaning) 
        => vocabulary
            .Where(v => v.Meanings.Contains(meaning.AsRepositoryString()))
            .Select(v => v.Word);

    public static string AsRepositoryString(this string str) => str.ToLowerInvariant().Trim();
}